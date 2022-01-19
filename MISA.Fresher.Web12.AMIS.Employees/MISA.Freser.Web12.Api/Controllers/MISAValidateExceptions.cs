using System.Runtime.Serialization;

namespace MISA.Fresher.Web12.Controllers
{
    [Serializable]
    internal class MISAValidateExceptions : Exception
    {
        public MISAValidateExceptions()
        {
        }

        public MISAValidateExceptions(string? message) : base(message)
        {
        }

        public MISAValidateExceptions(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MISAValidateExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}