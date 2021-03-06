﻿using System;
using System.Collections.Generic;
using System.Text;
using IFramework.Infrastructure;
using IFramework.Message;
using IFramework.Message.Impl;

namespace IFramework.MessageQueue.MSKafka.MessageFormat
{
    public class MessageContext : IMessageContext
    {
        public KafkaMessage KafkaMessage { get; protected set; }
        public long Offset { get; protected set; }
        public int Partition { get; protected set; }
        public List<IMessageContext> ToBeSentMessageContexts { get; protected set; }

        public MessageContext(KafkaMessage kafkaMessage, int partition, long offset)
        {
            KafkaMessage = kafkaMessage;
            Offset = offset;
            Partition = partition;
            ToBeSentMessageContexts = new List<IMessageContext>();
        }

        public MessageContext(object message, string id = null)
        {
            KafkaMessage = new KafkaMessage();
            SentTime = DateTime.Now;
            Message = message;
            if (!string.IsNullOrEmpty(id))
            {
                MessageID = id;
            }
            else if (message is IMessage)
            {
                MessageID = (message as IMessage).ID;
            }
            else
            {
                MessageID = ObjectId.GenerateNewId().ToString();
            }
            ToBeSentMessageContexts = new List<IMessageContext>();
            if (message != null && message is IMessage)
            {
                Topic = (message as IMessage).GetTopic();
            }
        }


        public MessageContext(IMessage message, string key)
            : this(message)
        {
            Key = key;
        }

        public MessageContext(IMessage message, string replyToEndPoint, string key)
            : this(message, key)
        {
            ReplyToEndPoint = replyToEndPoint;
        }

        public IDictionary<string, object> Headers
        {
            get { return KafkaMessage.Headers; }
        }

        public string Key
        {
            get { return (string)Headers.TryGetValue("Key"); }
            set { Headers["Key"] = value; }
        }

        public string CorrelationID
        {
            get { return (string)Headers.TryGetValue("CorrelationID"); }
            set { Headers["CorrelationID"] = value; }
        }

        public string MessageID
        {
            get { return (string)Headers.TryGetValue("MessageID"); }
            set { Headers["MessageID"] = value; }
        }

        public string ReplyToEndPoint
        {
            get { return (string)Headers.TryGetValue("ReplyToEndPoint"); }
            set { Headers["ReplyToEndPoint"] = value; }
        }

        public object Reply
        {
            get;
            set;
        }

        object _Message;
        public object Message
        {
            get
            {
                if (_Message != null)
                {
                    return _Message;
                }
                object messageType = null;
                if (Headers.TryGetValue("MessageType", out messageType) && messageType != null)
                {
                    var jsonValue = Encoding.UTF8.GetString(KafkaMessage.Payload);
                    _Message = jsonValue.ToJsonObject(Type.GetType(messageType.ToString()));

                }
                return _Message;
            }
            protected set
            {
                _Message = value;
                KafkaMessage.Payload = Encoding.UTF8.GetBytes(value.ToJson());
                if (value != null)
                {
                    Headers["MessageType"] = value.GetType().AssemblyQualifiedName;
                }
            }
        }

        public DateTime SentTime
        {
            get { return (DateTime)Headers.TryGetValue("SentTime"); }
            set { Headers["SentTime"] = value; }
        }

        public string Topic
        {
            get { return (string)Headers.TryGetValue("Topic"); }
            set { Headers["Topic"] = value; }
        }
    }
}
