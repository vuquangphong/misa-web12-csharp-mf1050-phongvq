using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Exceptions
{
    /// <summary>
    /// @author: VQPhong (18/01/2022).
    /// @modified: VQPhong (14/06/2022)
    /// @desc: (One of the custom exception handler) Definition of MISA Exceptions.
    /// </summary>
    public class MISAValidateException : Exception
    {
        // Method 1: Responding messages respectively
        private readonly string? _msgErrValidate = null;

        public MISAValidateException(string msgErrValidate)
        {
            this._msgErrValidate = msgErrValidate;
        }
        
        public override string Message => this._msgErrValidate;


        // Method 2: Responding all at once
        //private readonly IDictionary _listErrMsgs;

        //public MISAValidateException(List<string> listErrMsgs)
        //{
        //    _listErrMsgs = new Dictionary<string, List<string>>();
        //    _listErrMsgs.Add("ErrMsgs", listErrMsgs);
        //}

        //public override IDictionary Data => this._listErrMsgs;
    }
}
