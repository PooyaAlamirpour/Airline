using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using RestAirline.Booking.Domain.Booking;
using RestAirline.Booking.Domain.Booking.Trip;
using RestAirline.Booking.Domain.ModelBuilders;
using Xunit;

namespace RestAirline.Booking.Domain.Tests.Journeys
{
    public class SelectJourneyTests : TestBase
    {
        private List<Journey> _journeys;

        [Fact]
        public async Task AfterSelectedJourneysShouldHaveJourneys()
        {
            //Arrange
            _journeys = new JourneysBuilder().BuildJourneys();
            void SelectJourney(Booking.Booking b) => b.SelectJourneys(_journeys);

            //Act
            await UpdateAsync(BookingId, (Action<Booking.Booking>) SelectJourney);
            
            //Assert
            var booking =await AggregateStore.LoadAsync<Booking.Booking,BookingId>(BookingId,CancellationToken.None);
            booking.Journeys.Should().NotBeEmpty();
        }
    }
}