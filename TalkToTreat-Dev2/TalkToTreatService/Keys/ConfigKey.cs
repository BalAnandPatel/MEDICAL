using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.ProductImportAPI.Service.Keys
{
   public class ConfigKey
    {
        public static string OrgId = ConfigurationManager.AppSettings.Get("OrgId");
    }
}
