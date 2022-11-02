using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BaseApiApp.Entities.Account
{
    public class Appointment
    {
        public Guid Id { get; set; } 
        public string Body { get; set; }
        public string AppointmentNumber { get; set; }
        public string Subject { get; set; }
        public bool IsReplied { get; set; }
        public Guid EnquiryReplyId { get; set; }
        public Guid DoctorId { get; set; }
        public string FullName { get; set; }
        public string DoctorName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string AppointMentType { get; set; }
        public int PatientAge { get; set; }
        public DateTime AppointMentDate { get; set; }
        public int TotalRecord { get; set; }
        public string Country { get; set; }
        public string DoctorImage { get; set; }
        public decimal ConsultationFee { get; set; }
       // public List<EnquiryResponse> enquiryResponses { get; set; }

        #region "Line Items"
        //
        [XmlIgnore]
        public string EnquiryResponseXml { get; set; }

        private List<EnquiryResponse> _EnquiryResponse;
        public List<EnquiryResponse> enquiryResponses
        {
            get
            {
                if (_EnquiryResponse != null) return _EnquiryResponse;
                if (!string.IsNullOrEmpty(EnquiryResponseXml))
                {
                    _EnquiryResponse = (from o in XElement.Parse(EnquiryResponseXml).Descendants("OrderLine")
                                  select new EnquiryResponse
                                  {
                                      Id = o.Attribute("ID") != null ? Guid.Parse(o.Attribute("ID").Value) : Guid.Empty,
                                      Body = o.Attribute("Body") != null ? o.Attribute("Body").Value : "",
                                      Subject = o.Attribute("ProductStockCode") != null ? o.Attribute("Subject").Value : "",
                                      EnquiryReplyId = o.Attribute("EnquiryReplyId") != null ? Guid.Parse(o.Attribute("EnquiryReplyId").Value) : Guid.Empty,
                                      Created = o.Attribute("Created") != null ? Convert.ToDateTime(o.Attribute("Created").Value) : DateTime.Now,
                                      IsDoctorRemark = o.Attribute("IsDoctorRemark") != null ? Convert.ToBoolean(Convert.ToInt16(o.Attribute("IsDoctorRemark").Value)) : false,
                                  }).ToList();
                }
                return _EnquiryResponse;
            }
            set
            {
                EnquiryResponseXml = null;
                _EnquiryResponse = value;
            }


        }
        
       
        #endregion


    }

    public class EnquiryResponse
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; } 
        public Guid EnquiryReplyId { get; set; }
        public DateTime Created { get; set; } 
        public bool IsDoctorRemark { get; set; } 
    }
}
