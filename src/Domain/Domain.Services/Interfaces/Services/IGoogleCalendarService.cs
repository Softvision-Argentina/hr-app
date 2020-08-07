// <copyright file="IGoogleCalendarService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using Google.Apis.Calendar.v3.Data;

    public interface IGoogleCalendarService
    {
        string CreateEvent(Event newEvent);

        bool DeleteEvent(string googleCalendarEventId);
    }
}
