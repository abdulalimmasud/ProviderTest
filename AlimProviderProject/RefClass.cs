using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlimProviderProject
{
    public class RefClass
    {




        public static string AddCalenderEvents(string refreshToken, string calendarId)
        {
            string eventId = string.Empty;

            try
            {
                var calendarService = Models.SDKHelper.GoogleCalendarService(refreshToken);

                if (calendarService != null)
                {
                    Event myEvent = new Event
                    {
                        Summary = "Appointment",
                        Location = "Dhaka",
                        Start = new EventDateTime()
                        {
                            DateTime = new DateTime(2018, 9, 17, 10, 0, 0),
                            TimeZone = "Asia/Dhaka"
                        },
                        End = new EventDateTime()
                        {
                            DateTime = new DateTime(2018, 9, 17, 10, 30, 0),
                            TimeZone = "Asia/Dhaka"
                        },
                        Recurrence = new String[] {
                                "RRULE:FREQ=WEEKLY;BYDAY=MO"
                            },
                        Attendees = new List<EventAttendee>()
                        {
                          new EventAttendee() { Email = "alimcu08@gmail.com" }
                        }
                    };

                    var newEventRequest = calendarService.Events.Insert(myEvent, calendarId);

                    
                        //newEventRequest.SendNotifications = true;
                    var eventResult = newEventRequest.Execute();
                    eventId = eventResult.Id;
                }
            }
            catch (Exception ex)
            {
                eventId = string.Empty;
            }
            return eventId;
        }

        public static Google.Apis.Calendar.v3.Data.Event UpdateCalenderEvents(string refreshToken, string emailAddress, string summary, DateTime? start, DateTime? end, string eventId, out string error)
        {
            Google.Apis.Calendar.v3.Data.Event eventResult = null;
            error = string.Empty;
            string serviceError;
            try
            {
                var calendarService = Models.SDKHelper.GoogleCalendarService(refreshToken);
                if (calendarService != null)
                {
                    var list = calendarService.CalendarList.List().Execute();
                    var calendar = list.Items.SingleOrDefault(c => c.Summary == emailAddress);
                    if (calendar != null)
                    {
                        // Define parameters of request
                        EventsResource.ListRequest request = calendarService.Events.List("primary");
                        request.TimeMin = DateTime.Now;
                        request.ShowDeleted = false;
                        request.SingleEvents = true;
                        request.MaxResults = 10;
                        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                        // Get selected event
                        Google.Apis.Calendar.v3.Data.Events events = request.Execute();
                        var selectedEvent = events.Items.FirstOrDefault(c => c.Id == eventId);
                        if (selectedEvent != null)
                        {
                            selectedEvent.Summary = summary;
                            selectedEvent.Start = new Google.Apis.Calendar.v3.Data.EventDateTime
                            {
                                DateTime = start
                            };
                            selectedEvent.End = new Google.Apis.Calendar.v3.Data.EventDateTime
                            {
                                DateTime = start.Value.AddHours(12)
                            };
                            selectedEvent.Recurrence = new List<string>();

                            // Set Remainder
                            selectedEvent.Reminders = new Google.Apis.Calendar.v3.Data.Event.RemindersData()
                            {
                                UseDefault = false,
                                Overrides = new Google.Apis.Calendar.v3.Data.EventReminder[]
                                {
                                                        new Google.Apis.Calendar.v3.Data.EventReminder() { Method = "email", Minutes = 24 * 60 },
                                                        new Google.Apis.Calendar.v3.Data.EventReminder() { Method = "popup", Minutes = 24 * 60 }
                                }
                            };

                            // Set Attendees
                            selectedEvent.Attendees = new Google.Apis.Calendar.v3.Data.EventAttendee[]
                            {
                                                    new Google.Apis.Calendar.v3.Data.EventAttendee() { Email = "kaptan.cse@gmail.com" },
                                                    new Google.Apis.Calendar.v3.Data.EventAttendee() { Email = emailAddress }
                            };
                        }

                        var updateEventRequest = calendarService.Events.Update(selectedEvent, calendar.Id, eventId);
                        updateEventRequest.SendNotifications = true;
                        eventResult = updateEventRequest.Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                eventResult = null;
                error = ex.ToString();
            }
            return eventResult;
        }
        public static void DeletCalendarEvents(string refreshToken, string emailAddress, string eventId, out string error)
        {
            string result = string.Empty;
            error = string.Empty;
            string serviceError;
            try
            {
                var calendarService = Models.SDKHelper.GoogleCalendarService(refreshToken); ;
                if (calendarService != null)
                {
                    var list = calendarService.CalendarList.List().Execute();
                    var calendar = list.Items.FirstOrDefault(c => c.Summary == emailAddress);
                    if (calendar != null)
                    {
                        // Define parameters of request
                        EventsResource.ListRequest request = calendarService.Events.List("primary");
                        request.TimeMin = DateTime.Now;
                        request.ShowDeleted = false;
                        request.SingleEvents = true;
                        request.MaxResults = 10;
                        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                        // Get selected event
                        Google.Apis.Calendar.v3.Data.Events events = request.Execute();
                        var selectedEvent = events.Items.FirstOrDefault(c => c.Id == eventId);
                        if (selectedEvent != null)
                        {
                            var deleteEventRequest = calendarService.Events.Delete(calendar.Id, eventId);
                            deleteEventRequest.SendNotifications = true;
                            deleteEventRequest.Execute();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
                error = ex.ToString();
            }
        }
    }

}
