using System;

namespace InstarmCore.Utils
{
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
