using System;

namespace PonyUrl.Common
{
    /// <summary>
    /// Helps with validation
    /// </summary>
    public static class Check
    {
        public static void ArgumentNotNull(object obj, string paramName = "")
        {
            That<ArgumentNullException>(obj == null, paramName); 
        }

        public static void ArgumentNotNullOrEmpty(string text)
        {
            if (IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));
        }

        public static void NotNull(object obj, string paramName = "")
        {
            That<NullReferenceException>(obj == null, paramName);
        }

        public static void ArgumentNotUrl(string url)
        {
            if (!IsValidUrl(url))
            {
                throw new ArgumentException("Url is invalid format");
            }
        }

        public static void ArgumentNotDefaultOrEmpty(Guid guid)
        {
            if(IsGuidDefaultOrEmpty(guid))
            {
                throw new ArgumentException(nameof(guid));
            }
        }


        public static bool IsGuidDefaultOrEmpty(Guid guid)
        {
            return guid == Guid.Empty || guid == default(Guid);
        }

        public static bool IsNull(object obj)
        {
            return obj == null;
        }

        public static bool IsNullOrEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
                                 
        }

        public static bool IsNotNull(object obj)
        {
            return !IsNull(obj);
        }

        public static bool IsNotNullOrEmpty(string text)
        {
            return !IsNullOrEmpty(text);
        }

        public static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uri);
        }

        public static void That<TException>(bool condition, string message = "") where TException : Exception
        {
            if (condition)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            } 
        }



    }
}
