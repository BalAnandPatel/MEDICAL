using Newtonsoft.Json; 
using TalkToTreatService.Data; 
using TalkToTreatService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TalkToTreatService.Data.Dbml;

namespace TalkToTreatService.Data.DataImport
{
    /// <summary>
    /// Payment service
    /// </summary>
    public partial class UserRepository : IUserRepository
    {
        #region Fields 
        private readonly TalktoTreatClassesDataContext talktoTreatData;
        #endregion

        #region Ctor
        public UserRepository(IDataContextFactory dataContextFactory)
        {
            talktoTreatData = dataContextFactory.TalktoTreatDataContext();
        }


        #endregion

   
    }
}
