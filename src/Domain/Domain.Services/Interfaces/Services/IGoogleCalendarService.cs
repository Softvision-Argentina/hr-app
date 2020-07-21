using Google.Apis.Calendar.v3.Data;

namespace Domain.Services.Interfaces.Services
{
    public interface IGoogleCalendarService
    {
        string CreateEvent(Event newEvent);
        bool DeleteEvent(string googleCalendarEventId);
    }
}
