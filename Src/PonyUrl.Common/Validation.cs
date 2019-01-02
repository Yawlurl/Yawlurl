using System;

namespace PonyUrl.Common
{
    public static class Validation
    {
        public static void ArgumentNotNull(object obj, string paramName = "")
        {
            That<ArgumentNullException>(obj != null, paramName); 
        }

        public static void ArgumentNotNullOrEmpty(object obj)
        {
            if (IsNullOrEmpty(obj))
                throw new ArgumentNullException(nameof(obj));
        }

        public static void NotNull(object obj, string paramName = "")
        {
            That<NullReferenceException>(obj != null, paramName);
        }

        public static void ArgumentNotUrl(string url)
        {
            if (!IsValidUrl(url))
            {
                throw new ArgumentException("Url is invalid format");
            }
        }


        public static bool IsNull(object obj)
        {
            return obj == null;
        }

        public static bool IsNullOrEmpty(object obj)
        {
            return obj is string ? string.IsNullOrWhiteSpace(Convert.ToString(obj)) 
                                 : obj is Guid ? Guid.Empty == (Guid)obj 
                                               : obj == null;
        }

        public static bool IsNotNull(object obj)
        {
            return !IsNull(obj);
        }

        public static bool IsNotNullOrEmpty(object obj)
        {
            return !IsNullOrEmpty(obj);
        }

        public static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uri);
        }


        public static void That<TException>(bool condition, string message = "") where TException : Exception
        {
            if (!condition)
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }



    }
}
