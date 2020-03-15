using System;

namespace VerboseRestSharp.Exceptions
{
    /// <summary>
    /// Base exception thrown by VerboseRestSharp.
    /// </summary>
    public class VerboseRestSharpException : Exception
    {
        public VerboseRestSharpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
