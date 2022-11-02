using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading.Tasks;
using TalkToTreat.Models;
using System.Threading;
using Google.Apis.Util;

namespace TalkToTreat.Helper
{
    public class GoogleCelendarHelper
    {
        protected GoogleCelendarHelper()
        {
            
        }

        [System.Obsolete]
        public static async Task<Event> CreateGoogleCalendar(GoogleCalendar request, string creJson, string tokenJson )
        {
            string[] Scopes = { "https://www.googleapis.com/auth/calendar" };
            string ApplicationName = "Google Calendar Api";
            UserCredential credential;
                        
            using (var stream = new FileStream(creJson, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenJson, true)

                    ).Result;
            }

            if (credential.Token.IsExpired(SystemClock.Default))
            {
                credential.RefreshTokenAsync(CancellationToken.None).Wait();
            }


            //define services
            var services = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            //EventAttendee a = new EventAttendee();
            //a.Email = "talktotreatevent@gmail.com";
            //EventAttendee b = new EventAttendee();
            //b.Email = "govindsharma9875@gmail.com";
            //EventAttendee[] attendees = { a,b  };

            ConferenceData conferenceData = new ConferenceData();
            conferenceData.CreateRequest = new CreateConferenceRequest()
            {
                RequestId = request.RequestId,
                ConferenceSolutionKey = new ConferenceSolutionKey()
                {
                    Type = "hangoutsMeet",
                },                
            };          

         
            //define request
            Event eventCalendar = new Event()
            {
                Summary = request.Summary,
                Location = request.Location,
                Start = new EventDateTime
                {
                    DateTime = request.Start,
                    TimeZone = "Asia/Kolkata"
                },
                End = new EventDateTime
                {
                    DateTime = request.End,
                    TimeZone = "Asia/Kolkata"
                },
                Description = request.Description,
                Attendees= request.attendees,
                ConferenceData= conferenceData                
            };

            var eventRequest = services.Events.Insert(eventCalendar, "primary");
            eventRequest.ConferenceDataVersion = 1;
            var requestCreate = await eventRequest.ExecuteAsync();
            return requestCreate;
        }
    }
}
