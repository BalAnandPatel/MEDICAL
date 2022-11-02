using BaseApiApp.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiApp.Framework.Messages
{
    public class BooleanResponse
    {
        public AcknowledgeType Acknowledge = AcknowledgeType.Success;
        /// <summary>
        /// Message back to client. Mostly used when a web service failure occurs. 
        /// </summary>
        public string Message { get; set; }
        public string MessageCode { get; set; }
        public int StateId { get; set; }
        public bool IsValid { get; set; }
    }
}
