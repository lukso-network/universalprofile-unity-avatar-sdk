using System;

namespace UniversalProfileSDK
{
    internal class NullResponseException : Exception
    {
        public override string Message => message;
        string message;

        public NullResponseException() { }

        public NullResponseException(string message)
        {
            this.message = message;
        }
    }
}