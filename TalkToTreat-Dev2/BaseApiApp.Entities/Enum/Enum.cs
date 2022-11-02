using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiApp.Entities.Enum
{
    public enum UserType
    {
        Jobseeker = 1,
        Employer = 2
    }

    public enum AcknowledgeType
    {
        // Summary:
        //     Respresents an failed response.
        Failure = 0,
        //
        // Summary:
        //     Represents a successful response.
        Success = 1,
    }

    public enum AppointmentStatus
    {

                Pending = 0
              , Inqueue = 1
              , Scheduled = 2
    }

    public enum CancelledBY
    {
                User = 1
              , Doctor = 2
              , Admin = 3
    }
}