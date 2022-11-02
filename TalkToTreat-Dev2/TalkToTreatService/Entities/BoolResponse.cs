using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreatService.Entities
{
    public class BoolResponse : BaseEntity
    {
        public AcknowledgeType Acknowledge { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }

    }
}
