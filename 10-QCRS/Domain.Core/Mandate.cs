using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Core
{
    /// <summary>
    /// A static class which simplifies the basic checking of parameters.
    /// </summary>
    public static class Mandate
    {
        /// <summary>
        /// Mandates that the specified parameter is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null.</exception>
        public static void ParameterNotNull<T>(T value, string paramName) where T : class
        {
            if(value == null)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Mandates that the specified parameter is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null or whitespace.</exception>
        public static void ParameterNotNullOrEmpty(string value, string paramName)
        {
            ParameterNotNull(value, paramName);

            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("A non empty string is required", paramName);
        }

        /// <summary>
        /// Mandates that the specified value parameter may not be the default value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is the default value.</exception>
        public static void ParameterNotDefaut<T>(T value, string paramName) where T : struct, IEquatable<T>
        {
            if (value.Equals(default(T)))
            {
                throw new ArgumentException("A non-default value is required.", paramName);
            }
        }

        /// <summary>
        /// Mandates that the specified parameter is not null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is empty.</exception>
        public static void ParameterNotNullOrEmpty<T>(T value, string paramName) where T : class, IHaveIdentity
        {
            ParameterNotNull(value, paramName);

            if (value.IsEmpty())
            {
                throw new ArgumentException("A non-empty value is required.", paramName);
            }
        }

        /// <summary>
        /// Mandates that the specified sequence is not null and has at least one element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="paramName">Name of the param.</param>
        public static void ParameterNotNullOrEmpty<T>(IEnumerable<T> sequence, string paramName)
        {
            // ReSharper disable PossibleMultipleEnumeration
            ParameterNotNull(sequence, paramName);
            ParameterCondition(sequence.Any(), paramName, "May not be empty.");
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Mandates that the specified parameter matches the condition.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="ArgumentException">If the condition is false.</exception>
        public static void ParameterCondition(bool condition, string paramName)
        {
            if(!condition)
                throw new ArgumentException(paramName);
        }

        /// <summary>
        /// Mandates that the specified parameter matches the condition.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">If the condition is false.</exception>
        public static void ParameterCondition(bool condition, string paramName, string message)
        {
            if(!condition)
                throw new ArgumentException(message, paramName);
        }

        /// <summary>
        /// Mandates that the specified condition is true, otherwise throws an exception specified in <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="condition">if set to <c>true</c>, throws exception <typeparamref name="TException"/>.</param>
        /// <exception cref="Exception">An exception of type <typeparamref name="TException"/> is raised if the condition is false.</exception>
        public static void That<TException>(bool condition) where TException : Exception, new()
        {
            if (!condition)
                throw Activator.CreateInstance<TException>();
        }

        /// <summary>
        /// Mandates that the specified condition is true, otherwise throws an exception specified in <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="condition">if set to <c>true</c>, throws exception <typeparamref name="TException"/>.</param>
        /// <param name="message">The message to place in the exception</param>
        /// <exception cref="Exception">An exception of type <typeparamref name="TException"/> is raised if the condition is false.</exception>
        public static void That<TException>(bool condition, string message) where TException : Exception, new()
        {
            if (condition)
                return;

            var exception = Activator.CreateInstance(typeof (TException), message) as TException;

            if (exception == null)
            {
                throw new Exception(String.Format("Unable to create an exception of type {0}",
                    typeof (TException).FullName));
            }

            throw exception;
        }

        /// <summary>
        /// Mandates that the specified condition is true, otherwise throws an Argument exception
        /// </summary>
        public static void That(bool condition, string message)
        {
            if (!condition)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Mandates that the specified condition is true, otherwise throws an Argument exception
        /// </summary>
        public static void That(bool condition)
        {
            if (!condition)
                throw new ArgumentException();
        }

        /// <summary>
        /// Mandates that the specified condition is true, otherwise throws an exception specified in <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="condition">if set to <c>true</c>, throws exception <typeparamref name="TException"/>.</param>
        /// <param name="defer">Deffered expression to call if the exception should be raised.</param>
        /// <exception cref="Exception">An exception of type <typeparamref name="TException"/> is raised if the condition is false.</exception>
        public static void That<TException>(bool condition, Func<TException> defer) where TException : Exception, new()
        {
            if (!condition)
            {
                throw defer.Invoke();
            }

            // Here is an example of how this method is actually called
            //object myParam = null;
            //Mandate.That(myParam != null, () => new ArgumentNullException("myParam")));
        }
    }
}
