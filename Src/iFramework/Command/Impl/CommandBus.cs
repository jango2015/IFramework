﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFramework.Infrastructure;
using IFramework.Message;
using IFramework.UnitOfWork;
using IFramework.Message.Impl;
using System.Collections;
using System.Threading;
using IFramework.Config;
using IFramework.Infrastructure.Logging;
using IFramework.MessageQueue;
using System.Collections.Concurrent;
using IFramework.SysExceptions;
using IFramework.IoC;
using IFramework.Infrastructure.Mailboxes.Impl;

namespace IFramework.Command.Impl
{
    public class CommandBus : MessageSender, ICommandBus
    {
        protected string _replyTopicName;
        protected string _replySubscriptionName;
        //protected string[] _commandQueueNames;
        protected ILinearCommandManager _linearCommandManager;
        /// <summary>
        /// cache command states for command reply. When reply comes, make replyTaskCompletionSouce completed
        /// </summary>
        protected ConcurrentDictionary<string, MessageState> _commandStateQueues;
        protected MessageProcessor _messageProcessor;
        protected Action<IMessageContext> _removeMessageContext;
        protected string _consumerId;
        public CommandBus(IMessageQueueClient messageQueueClient,
                          ILinearCommandManager linearCommandManager,
                          string consumerId,
                          string replyTopicName,
                          string replySubscriptionName)
            : base(messageQueueClient)
        {
            _consumerId = consumerId;
            _commandStateQueues = new ConcurrentDictionary<string, MessageState>();
            _linearCommandManager = linearCommandManager;
            _replyTopicName = Configuration.Instance.FormatAppName(replyTopicName);
            _replySubscriptionName = Configuration.Instance.FormatAppName(replySubscriptionName);
            _messageProcessor = new MessageProcessor(new DefaultProcessingMessageScheduler<IMessageContext>());
        }
        protected override IEnumerable<IMessageContext> GetAllUnSentMessages()
        {
            using (var scope = IoCFactory.Instance.CurrentContainer.CreateChildContainer())
            using (var messageStore = scope.Resolve<IMessageStore>())
            {
                return messageStore.GetAllUnSentCommands((messageId, message, topic, correlationId) =>
                                                          _messageQueueClient.WrapMessage(message, key: message.Key, topic: topic, messageId: messageId, correlationId: correlationId));
            }
        }

        protected override void Send(IMessageContext messageContext, string queue)
        {
            _messageQueueClient.Send(messageContext, queue);
        }

        protected override void CompleteSendingMessage(MessageState messageState)
        {
            messageState?.SendTaskCompletionSource?
                         .TrySetResult(new MessageResponse(messageState.MessageContext,
                                                           messageState.ReplyTaskCompletionSource?.Task,
                                                           messageState.NeedReply));

            if (_needMessageStore)
            {
                Task.Run(() =>
                {
                    using (var scope = IoCFactory.Instance.CurrentContainer.CreateChildContainer())
                    using (var messageStore = scope.Resolve<IMessageStore>())
                    {
                        messageStore.RemoveSentCommand(messageState.MessageID);
                    }
                });
            }
        }

        public override void Start()
        {
            base.Start();
            #region init process command reply worker
            try
            {
                if (!string.IsNullOrWhiteSpace(_replyTopicName))
                {
                    _removeMessageContext = _messageQueueClient.StartSubscriptionClient(_replyTopicName, _replySubscriptionName, _consumerId, OnMessagesReceived);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.GetBaseException().Message, e);
            }
            #endregion
            _messageProcessor.Start();
        }

        public override void Stop()
        {
            base.Stop();
            _messageProcessor.Stop();
        }

        protected void OnMessagesReceived(params IMessageContext[] replies)
        {
            replies.ForEach(reply =>
            {
                _messageProcessor.Process(reply, ConsumeReply);
            });
        }

        protected async Task ConsumeReply(IMessageContext reply)
        {
            _logger.InfoFormat("Handle reply:{0} content:{1}", reply.MessageID, reply.ToJson());
            var messageState = _commandStateQueues.TryGetValue(reply.CorrelationID);
            if (messageState != null)
            {
                _commandStateQueues.TryRemove(reply.CorrelationID);
                if (reply.Message is Exception)
                {
                    messageState.ReplyTaskCompletionSource.TrySetException(reply.Message as Exception);
                }
                else
                {
                    messageState.ReplyTaskCompletionSource.TrySetResult(reply.Message);
                }
            }
            _removeMessageContext(reply);
        }

        protected MessageState BuildCommandState(IMessageContext commandContext, CancellationToken sendCancellationToken, TimeSpan timeout, CancellationToken replyCancellationToken, bool needReply)
        {
            var sendTaskCompletionSource = new TaskCompletionSource<MessageResponse>();
            if (timeout != TimerTaskFactory.Infinite)
            {
                var timeoutCancellationTokenSource = new CancellationTokenSource(timeout);
                timeoutCancellationTokenSource.Token.Register(OnSendTimeout, sendTaskCompletionSource);
            }

            if (sendCancellationToken != CancellationToken.None)
            {
                sendCancellationToken.Register(OnSendCancel, sendTaskCompletionSource);
            }

            TaskCompletionSource<object> replyTaskCompletionSource = null;
            MessageState commandState = null;
            if (needReply)
            {
                replyTaskCompletionSource = new TaskCompletionSource<object>();
                commandState = new MessageState(commandContext, sendTaskCompletionSource, replyTaskCompletionSource, needReply);
                if (replyCancellationToken != CancellationToken.None)
                {
                    replyCancellationToken.Register(OnReplyCancel, commandState);
                }
            }
            else
            {
                commandState = new MessageState(commandContext, sendTaskCompletionSource, needReply);
            }
            return commandState;
        }

        public Task<MessageResponse> SendAsync(ICommand command, bool needReply = false)
        {
            return SendAsync(command, CancellationToken.None, TimerTaskFactory.Infinite, CancellationToken.None, needReply);
        }

        public Task<MessageResponse> SendAsync(ICommand command, TimeSpan timeout, bool needReply = false)
        {
            return SendAsync(command, CancellationToken.None, timeout, CancellationToken.None, needReply);
        }

        public Task<MessageResponse> SendAsync(ICommand command, CancellationToken sendCancellationToken, TimeSpan timeout, CancellationToken replyCancellationToken, bool needReply = false)
        {
            var commandContext = WrapCommand(command, needReply);
            var commandState = BuildCommandState(commandContext, sendCancellationToken, timeout, replyCancellationToken, needReply);
            if (needReply)
            {
                _commandStateQueues.GetOrAdd(commandState.MessageID, commandState);
            }
            SendAsync(commandState);
            return commandState.SendTaskCompletionSource.Task;
        }

        public IMessageContext WrapCommand(ICommand command, bool needReply = false)
        {
            if (string.IsNullOrEmpty(command.ID))
            {
                _logger.Error(new NoMessageId());
                throw new NoMessageId();
            }
            string commandKey = null;
            if (command is ILinearCommand)
            {
                var linearKey = _linearCommandManager.GetLinearKey(command as ILinearCommand);
                if (linearKey != null)
                {
                    commandKey = linearKey.ToString();
                }
            }
            IMessageContext commandContext = null;
            #region pickup a queue to send command
            // move this logic into  concrete messagequeueClient. kafka sdk already implement it. 
            // service bus client still need it.
            //int keyUniqueCode = !string.IsNullOrWhiteSpace(commandKey) ?
            //    commandKey.GetUniqueCode() : command.ID.GetUniqueCode();
            //var queue = _commandQueueNames[Math.Abs(keyUniqueCode % _commandQueueNames.Length)];
            #endregion
            commandContext = _messageQueueClient.WrapMessage(command,
                                                             key: commandKey,
                                                             replyEndPoint: needReply ? _replyTopicName : null);
            if (string.IsNullOrEmpty(commandContext.Topic))
            {
                throw new NoCommandTopic();
            }
            return commandContext;
        }

        protected void OnSendTimeout(object state)
        {
            var sendTaskCompletionSource = state as TaskCompletionSource<MessageResponse>;
            if (sendTaskCompletionSource != null)
            {
                sendTaskCompletionSource.TrySetException(new TimeoutException());
            }
        }



        protected void OnReplyCancel(object state)
        {
            var messageState = state as MessageState;
            if (messageState != null)
            {
                messageState.ReplyTaskCompletionSource.TrySetCanceled();
                _commandStateQueues.TryRemove(messageState.MessageID);
            }
        }

        public void SendMessageStates(IEnumerable<MessageState> commandStates)
        {
            SendAsync(commandStates.ToArray());
        }
    }
}
