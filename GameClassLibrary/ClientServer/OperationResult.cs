using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary.ClientServer
{
    [DataContract]
    public struct OperationResult
    {
        [DataMember]
        public string Message { get; private set; }

        [DataMember]
        public bool Success { get; private set; }

        public OperationResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public static OperationResult Default()
        {
            return new OperationResult("Default message", true);
        }

        public static OperationResult NotImplemented()
        {
            return new OperationResult("Not Implemented", false);
        }

        public static OperationResult FailOperationResult(string message)
        {
            return new OperationResult(message, false);
        }

        public static OperationResult SuccesOperationResult(string message = "")
        {
            return new OperationResult(message, true);
        }
    }
}
