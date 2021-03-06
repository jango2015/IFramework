﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace IFramework.Specifications
{
    /// <summary>
    /// Represents the ALL specification which indicates that it
    /// will be satisified by all the objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Semantics(Semantics.All)]
    public sealed class AllSpecification<T> : Specification<T>
      //  where T : class, IEntity
    {
        #region Public Methods
        /// <summary>
        /// Returns a LINQ expression which represents the semantics
        /// of the specification.
        /// </summary>
        /// <returns>The LINQ expression.</returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            return o => true;
        }
        #endregion
    }
}
