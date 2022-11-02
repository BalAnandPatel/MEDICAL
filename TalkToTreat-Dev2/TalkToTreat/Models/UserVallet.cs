using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToTreat.Models
{
    public class UserVallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; } 
        public string Type { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        public int Save()
        {
            int returnbyproc = 0;

            try
            {
                var walletJson = JsonConvert.SerializeObject(this);
                _Mysave _mysave = new _Mysave("procJsonUpsertVallet_New");
                _mysave.AddParameter("@UserId", this.UserId.ToString());
                _mysave.AddParameter("@ValletJson", walletJson);
                _mysave.AddParameter("@SavedBy", "");

                _mysave.AddParameter("@FileType", this.Type);
                _mysave.AddParameter("@Description", this.Description);
                _mysave.AddParameter("@FileUrl", this.FileUrl);
                _mysave.AddParameter("@FileName", this.FileName);
                _mysave.AddParameter("@Id", Guid.NewGuid().ToString());

                returnbyproc = _mysave.ExecuteSave();
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
            return returnbyproc;
        }
        public int Delete()
        {
            int returnbyproc = 0;

            try
            {
                _Mysave _mysave = new _Mysave("ProcDeleteObject");
                _mysave.AddParameter("@ObjectId", Id.ToString());
                _mysave.AddParameter("@ObjectType", 4);
                returnbyproc = _mysave.ExecuteSave();
            }
            catch (Exception e)
            {

            }
            finally
            {


            }
            return returnbyproc;
        }

    }
}
