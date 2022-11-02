﻿using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;

namespace TalkToTreat.Models
{
    public class GoogleCalendar
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Start { get; set; }
        public DateTime End  { get; set; }

        public string RequestId { get; set; }

        public List<EventAttendee> attendees { get; set; }
    }
}
