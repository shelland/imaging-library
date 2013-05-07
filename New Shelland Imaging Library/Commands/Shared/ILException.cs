using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shelland.ImagingLibrary
{

    /// <summary>
    /// Base exception class for Imaging Library
    /// </summary>
    class ILException: Exception
    {

        public ILException(string ErrorMessage, string ErrorCode)
        {
            errorMsg = ErrorMessage;
            eCode = ErrorCode;
        }

        public ILException(string ErrorMessage)
        {
            errorMsg = ErrorMessage;
        }

        public ILException(string ErrorMessage, string ErrorCode, string HelpLink)
        {
            helpLink = HelpLink;
            errorMsg = ErrorMessage;
            eCode = ErrorCode;
        }

        string errorMsg = "";
        string eCode = "";
        string helpLink = "";

        public override System.Collections.IDictionary Data
        {
            get
            {
                Dictionary<string, string> DataDict = new Dictionary<string, string>();
                DataDict["ReleaseType"] = "DEBUG_VERSION";
                return DataDict;
            }
        }

        public override string Source
        {
            get
            {
                return "Shelland Imaging Library v0.2";
            }
        }

        public override string Message
        {
            get
            {
                return errorMsg;
            }
        }

        public override string HelpLink
        {
            get
            {
                return helpLink;
            }
        }

    }
}
