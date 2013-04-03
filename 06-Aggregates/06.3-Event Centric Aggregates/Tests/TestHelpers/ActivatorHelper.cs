using System;
using System.Linq;
using System.Reflection;

namespace Tests.TestHelpers
{
    public static class ActivatorHelper
    {
        const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

        public static T CreateInstance<T>(params object[] parameters) where T : class
        {
            Type[] types = parameters.ToList().ConvertAll(input => input.GetType()).ToArray();

            var constructor = typeof(T).GetConstructor(Flags, null, types, null);

            return constructor.Invoke(parameters) as T;
        }
    }
}