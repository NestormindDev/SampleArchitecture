using System;
using System.Runtime.Serialization;

namespace Architecture.Domain.Exceptions
{
    [Serializable]
    public class ArchitectureDomainException : Exception
    {
        public ArchitectureDomainException()
        {
        }

        public ArchitectureDomainException(string message) : base(message)
        {
        }

        public ArchitectureDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ArchitectureDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}