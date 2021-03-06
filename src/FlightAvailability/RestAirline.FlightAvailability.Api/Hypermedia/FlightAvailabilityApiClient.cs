﻿using System;
using System.Threading.Tasks;

namespace RestAirline.FlightAvailability.Api.Hypermedia
{
    public class FlightAvailabilityApiClient
    {
        private readonly FlightAvailabilityApiNavigator _flightAvailabilityApiNavigator;

        public FlightAvailabilityApiClient(FlightAvailabilityApiNavigator flightAvailabilityApiNavigator)
        {
            _flightAvailabilityApiNavigator = flightAvailabilityApiNavigator;
        }

        public Task<Guid> CreateBooking()
        {
//            //1. get home resource
//            var homeResource = await _restAirlineApiNavigator.Execute();
//
//            //2. get SearchTripsCommand from home resource
//            var searchTripsCommand = homeResource.ResourceCommands.SearchTripsCommand;
//            searchTripsCommand.SearchCriteria = TripSearchCriteria.DefaultTripSearchCriteria();
//
//            //3. get TripAvailabilityResource by posting searchTripsCommand
//            var tripAvailabilityResource = await _restAirlineApiNavigator.PostCommand(searchTripsCommand);
//
//            //4. get SelectTripCommand from TripAvailabilityResource
//            var selectTripCommand = tripAvailabilityResource.ResourceCommands.SelectTripCommand;
//            selectTripCommand.Trip = tripAvailabilityResource.AvailableTrips.First();
//
//            //5. get bookingId from booking resource
//            var bookingResource = await _restAirlineApiNavigator.PostCommand(selectTripCommand);
//            return bookingResource.BookingId;

            return Task.FromResult(Guid.NewGuid());
        }
    }
}