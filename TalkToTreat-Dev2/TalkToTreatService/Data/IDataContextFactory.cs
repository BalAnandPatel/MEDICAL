using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkToTreatService.Data.Dbml;

namespace TalkToTreatService.Data
{
    public partial interface IDataContextFactory
    {
        TalktoTreatClassesDataContext TalktoTreatDataContext();
    }
}
