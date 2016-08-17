using System;
using System.Linq;
using System.Reflection;

namespace SpecFlow.XFormsExtensions
{
    public static class Extensions
    {
        /// <summary>
        /// The get underlying type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type GetUnderlyingType(this Type type)
        {
            return type.IsGenericType ? type.GenericTypeArguments[0].GetUnderlyingType() : type;
        }
    }
}
