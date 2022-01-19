using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Exceptions
{
    public class MISAValidateException : Exception
    {
        private string? msgErrValidate = null;

        public MISAValidateException(string msgErrValidate)
        {
            this.msgErrValidate = msgErrValidate;
        }

        public override string Message
        {
            get { return msgErrValidate; }
        }
    }
}
