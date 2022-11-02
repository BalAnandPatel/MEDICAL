using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Linq.Mapping;
using System.Configuration;
using TalkToTreatService.Data.Dbml;

namespace TalkToTreatService.Data
{
    public partial class DataContextFactory : IDataContextFactory
    {
        static MappingSource _mappingSource = new AttributeMappingSource();
        static MappingSource _sharedMappingSource = new AttributeMappingSource();
        string _connectionString; 
        public TalktoTreatClassesDataContext TalktoTreatDataContext()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["TalktoTreatConnectionString"].ConnectionString;
            return new TalktoTreatClassesDataContext(_connectionString, _sharedMappingSource);
        }
    }
}
