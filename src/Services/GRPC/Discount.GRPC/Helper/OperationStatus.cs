using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.GRPC.Helper
{
    public class OperationStatus
    {
        public OperationStatus(string message, bool statusSuccess)
        {
            Message = message;
            StatusSuccess = statusSuccess;
        }

        public string Message { get; set; }
        public bool StatusSuccess { get; set; }
    }
}
