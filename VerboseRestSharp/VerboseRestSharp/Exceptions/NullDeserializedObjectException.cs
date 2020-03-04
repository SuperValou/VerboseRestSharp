using System;

namespace VerboseRestSharp.Exceptions
{
    public class NullResponseObjectException : VerboseRestSharpException
    {
        public Type ExpectedType { get; }

        public NullResponseObjectException(Type expectedType) 
            : base($"The expected '{expectedType?.Name}' object was null.", innerException: null)
        {
            ExpectedType = expectedType;
        }        
    }
}
