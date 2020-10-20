using CQELight.Tools;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CQELight.DAL.Mapping
{
    /// <summary>
    /// Type mapper for create DAL mapping without attributes
    /// </summary>
    public class TypeMapper<T>
    {
        #region Members

        internal readonly Type objectType;
        internal MemberInfo? idField;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new TypeMapper for a specific type.
        /// </summary>
        public TypeMapper()
        {
            objectType = typeof(T);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Defines the Id Field and returns the mapper instance.
        /// </summary>
        /// <returns>Curent instance of type mapper</returns>
        public TypeMapper<T> HasId(Expression<Func<T, object>> idFieldExpr)
        {
            var member = idFieldExpr.GetMemberInfo();
            if (member == null)
            {
                throw new InvalidOperationException("You specified an incorrect field for HasId");
            }
            idField = member;
            return this;
        }

        #endregion
    }
}
