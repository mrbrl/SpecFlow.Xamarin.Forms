using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Specflow.XForms.Helpers
{
    public static class ReflectionHelpers
    {

        /// <summary>
        /// ExecuteGenericMethod
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="genericType">
        /// </param>
        /// <param name="methodName">
        /// </param>
        /// <param name="args">
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteGenericMethod(
            object obj,
            Type type,
            Type genericType,
            string methodName,
            params object[] args)
        {
            try
            {
                MethodInfo method = null;

                try
                {
                    method = type.GetRuntimeMethods().SingleOrDefault(x => x.Name == methodName);
                }
                finally
                {
                    if (args != null && method == null)
                    {
                        Type[] types = args.Select(o => o?.GetType() ?? typeof(object)).ToArray();
                        method = type.GetMethodExt(methodName, types);
                    }
                }

                if (method == null)
                    throw new Exception($"Can't find method '{methodName}' on type '{type.FullName}'");

                MethodInfo generic = method.MakeGenericMethod(genericType);
                return generic.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                throw;
            }
        }

        /// <summary>
        /// Search for a method by name, parameter types, and binding flags.  Unlike GetMethod(), does 'loose' matching on
        ///     generic
        ///     parameter types, and searches base interfaces.
        /// </summary>
        /// <param name="thisType">
        /// The this Type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="parameterTypes">
        /// The parameter Types.
        /// </param>
        /// <exception cref="AmbiguousMatchException">
        /// </exception>
        /// <returns>
        /// The <see cref="MethodInfo"/>.
        /// </returns>
        public static MethodInfo GetMethodExt(this Type thisType, string name, params Type[] parameterTypes)
        {
            MethodInfo matchingMethod = null;

            // Check all methods with the specified name, including in base classes
            GetMethodExt(ref matchingMethod, thisType, name, parameterTypes);

            // If we're searching an interface, we have to manually search base interfaces
            if (matchingMethod == null && thisType.GetTypeInfo().IsInterface)
            {
                foreach (Type interfaceType in thisType.GetTypeInfo().ImplementedInterfaces)
                {
                    GetMethodExt(ref matchingMethod, interfaceType, name, parameterTypes);
                }
            }

            return matchingMethod;
        }

        /// <summary>
        /// The get method ext.
        /// </summary>
        /// <param name="matchingMethod">
        /// The matching method.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="parameterTypes">
        /// The parameter types.
        /// </param>
        /// <exception cref="AmbiguousMatchException">
        /// </exception>
        private static void GetMethodExt(
            ref MethodInfo matchingMethod,
            Type type,
            string name,
            params Type[] parameterTypes)
        {
            // Check all methods with the specified name, including in base classes
            foreach (MethodInfo methodInfo in type.GetRuntimeMethods().Where(x => x.Name == name))
            {
                // Check that the parameter counts and types match, with 'loose' matching on generic parameters
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                if (parameterInfos.Length == parameterTypes.Length)
                {
                    int i = 0;
                    for (; i < parameterInfos.Length; ++i)
                    {
                        if (!parameterInfos[i].ParameterType.IsSimilarType(parameterTypes[i]))
                        {
                            break;
                        }
                    }

                    if (i == parameterInfos.Length)
                    {
                        if (matchingMethod == null)
                        {
                            matchingMethod = methodInfo;
                        }
                        else
                        {
                            throw new AmbiguousMatchException("More than one matching method found!");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the two types are either identical, or are both generic parameters or generic types
        ///     with generic parameters in the same locations (generic parameters match any other generic paramter,
        ///     but NOT concrete types).
        /// </summary>
        /// <param name="thisType">
        /// The this Type.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsSimilarType(this Type thisType, Type type)
        {
            // Ignore any 'ref' types
            if (thisType.IsByRef)
            {
                thisType = thisType.GetElementType();
            }

            if (type.IsByRef)
            {
                type = type.GetElementType();
            }

            // Handle array types
            if (thisType.IsArray && type.IsArray)
            {
                return thisType.GetElementType().IsSimilarType(type.GetElementType());
            }

            if (thisType.Name == "System.RuntimeType" && type.Name == "System.Type")
            {
                return true;
            }

            // If the types are identical, or they're both generic parameters or the special 'T' type, treat as a match
            if (thisType == type
                || ((thisType.IsGenericParameter || thisType == typeof(T))
                    && (type.IsGenericParameter || type == typeof(T))))
            {
                return true;
            }

            // Handle any generic arguments
            if (thisType.GetTypeInfo().IsGenericType && type.GetTypeInfo().IsGenericType)
            {
                Type[] thisArguments = thisType.GenericTypeArguments;
                Type[] arguments = type.GenericTypeArguments;
                if (thisArguments.Length == arguments.Length)
                {
                    for (int i = 0; i < thisArguments.Length; ++i)
                    {
                        if (!thisArguments[i].IsSimilarType(arguments[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    ///     Special type used to match any generic parameter type in GetMethodExt().
    /// </summary>
    public class T
    {

    }
}
