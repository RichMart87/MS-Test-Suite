using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Utilities
{
    public static class AssertLogger
    {
        public static void LogAssert(string message)
        {
            // Log the assertion message to a file or console
            Console.WriteLine($"Assertion: {message}");
        }

        public static void LogAssert(string message, bool condition)
        {
            // Log the assertion message with condition
            Console.WriteLine($"Assertion: {message}, Condition: {condition}");
        }

        public static void LogAssert(string message, Exception ex)
        {
            // Log the assertion message with exception details
            Console.WriteLine($"Assertion: {message}, Exception: {ex.Message}");
        }

        public static void AreEqual<T>(T expected, T actual, string message)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. Expected: {expected}, Actual: {actual}");
            }
        }

        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. Condition was false.");
            }
        }

        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. Condition was true.");
            }
        }

        public static void IsNotNull<T>(T obj, string message)
        {
            if (obj == null)
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. Object was null.");
            }
        }

        public static void IsNull<T>(T obj, string message)
        {
            if (obj != null)
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. Object was not null.");
            }
        }

        public static void IsEmpty(string str, string message)
        {
            if (!string.IsNullOrEmpty(str))
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. String was not empty.");
            }
        }

        public static void IsNotEmpty(string str, string message)
        {
            if (string.IsNullOrEmpty(str))
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. String was empty.");
            }
        }

        public static void IsNotEqual<T>(T expected, T actual, string message)
        {
            if (EqualityComparer<T>.Default.Equals(expected, actual))
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. Expected: {expected}, Actual: {actual}");
            }
        }

        public static void IsNotSame<T>(T expected, T actual, string message)
        {
            if (ReferenceEquals(expected, actual))
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. Expected: {expected}, Actual: {actual}");
            }
        }

        public static void IsSame<T>(T expected, T actual, string message)
        {
            if (!ReferenceEquals(expected, actual))
            {
                LogAssert(message);
                throw new Exception($"Assertion failed: {message}. Expected: {expected}, Actual: {actual}");
            }
        }

        public static void IsNotSame<T>(T expected, T actual)
        {
            if (ReferenceEquals(expected, actual))
            {
                throw new Exception($"Assertion failed. Expected: {expected}, Actual: {actual}");
            }
        }

        public static void IsSame<T>(T expected, T actual)
        {
            if (!ReferenceEquals(expected, actual))
            {
                throw new Exception($"Assertion failed. Expected: {expected}, Actual: {actual}");
            }
        }
    }
}