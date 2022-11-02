using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreatService.Entities
{
    public enum AcknowledgeType
    {
        Error = 0,
        Success = 1
    }
    public enum Source
    {
        API=1,
        WEBJOB=2
    }
    public enum ImportStage
    {
        Raw = 0,
        Staging = 1,
        Live = 2

    }
    public enum ImportStatus
    {
        Created = 0,
        Processing = 1,
        Success = 2,
        Error = 3,
        PartiallyUpdated = 4,
        Scheduled = 5
    }
    public enum ImportDataType
    {
        Product = 0,
        Category = 1,
        Pricelist = 2, 
        Inventory = 3,
        Relationship = 4,
        CustomAttribute = 5,
        Variants = 6,
        Unknown = 10
    }
}
