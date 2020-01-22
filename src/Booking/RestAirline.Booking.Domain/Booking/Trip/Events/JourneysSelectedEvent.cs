using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace RestAirline.Booking.Domain.Booking.Trip.Events
{
    [EventVersion("JourneysSelected", 1)]
    public class JourneysSelectedEvent : AggregateEvent<Booking, BookingId>
    {
        public JourneysSelectedEvent()
        {
            
        }
        
        public JourneysSelectedEvent(List<Journey> journeys)
        {
            Journeys = journeys;
        }
        
        public List<Journey> Journeys { get; set; }
    }
}