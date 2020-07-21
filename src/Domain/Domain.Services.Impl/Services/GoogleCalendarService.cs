using Domain.Services.Interfaces.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

namespace Domain.Services.Impl.Services
{
    // TODO: Are those comments necessary? If not, delete.
    // Also check out try-catch statements, they return values instead of throwing exceptions.
    public class GoogleCalendarService : IGoogleCalendarService
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        public CalendarService InitializeCalendarService()
        {
            try
            {
                UserCredential credential;

                using (var stream =
                  new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                                        Scopes,
                                        "user",
                                        CancellationToken.None,
                                        new FileDataStore(credPath, true)).Result;
                }

                // Create Google Calendar API service.
                return new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string CreateEvent(Event newEvent)
        {
            try
            {
                var e = InitializeCalendarService().Events.Insert(newEvent, "primary").Execute();

                return e.Id;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public bool DeleteEvent(string googleCalendarEventId)
        {
            try
            {
                InitializeCalendarService().Events.Delete("primary", googleCalendarEventId).Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }


}
