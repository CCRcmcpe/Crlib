using System;
using System.Collections;
using System.Collections.Specialized;
using REVUnit.Crlib.Input.Properties;

namespace REVUnit.Crlib.Input
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string? faultedToken, string? message = null)
        {
            Message = message ?? string.Format(Resources.InvalidTokenException_Message, FaultedToken);
            FaultedToken = faultedToken;
        }

        public string? FaultedToken { get; set; }
        public override string Message { get; }

        public override IDictionary Data => new ListDictionary { { "FaultedToken", FaultedToken } };
    }
}