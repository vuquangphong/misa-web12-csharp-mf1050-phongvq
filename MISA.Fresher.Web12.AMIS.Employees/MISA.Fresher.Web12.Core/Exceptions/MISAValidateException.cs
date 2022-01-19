using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Exceptions
{
    /// <summary>
    /// @author: Vũ Quang Phong (18/01/2022)
    /// @desc: Definition of MISA Exceptions
    /// </summary>
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
