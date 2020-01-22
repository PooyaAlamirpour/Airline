﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.EntityFramework;
using EventFlow.Queries;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAirline.Booking.Api.Resources.Booking;
using RestAirline.Booking.Api.Resources.Booking.Journey;
using RestAirline.Booking.Api.Resources.Booking.Passenger.Add;
using RestAirline.Booking.Api.Resources.Booking.Passenger.Update;
using RestAirline.Booking.Domain.Booking;
using RestAirline.Booking.Domain.ModelBuilders;
using RestAirline.Booking.Queries.EntityFramework.Booking;
using RestAirline.Booking.ReadModel.EntityFramework.DBContext;
using BookingReadModel = RestAirline.Booking.ReadModel.InMemory.BookingReadModel;
using UpdatePassengerNameCommand = RestAirline.Booking.Commands.Passenger.UpdatePassengerNameCommand;

namespace RestAirline.Booking.Api.Controllers
{
    [Route("api/booking")]
    public class BookingController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IDbContextProvider<RestAirlineReadModelContext> _contextProvider;

        public BookingController(ICommandBus commandBus, IQueryProcessor queryProcessor, IDbContextProvider<RestAirlineReadModelContext> contextProvider)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
            _contextProvider = contextProvider;
        }

        [Route("journeys")]
        [HttpPost]
        public async Task<JourneysSelectedResource> SelectJourneys(SelectJourneysCommand selectJourneysCommand)
        {
            //Will integrate journey availability micro-service before select journey, passenger query journey availability micro-service in UI
            //Get journey from journey availability micro-service
            var journeys = new JourneysBuilder().BuildJourneys();
            var bookingId = BookingId.New;

            var command = new Booking.Commands.Journey.SelectJourneysCommand(bookingId, journeys);
            await _commandBus.PublishAsync(command, CancellationToken.None);

            return new JourneysSelectedResource(Url, bookingId.Value);
        }

        [Route("{bookingId}")]
        [HttpGet]
        public async Task<BookingResource> GetBooking(string bookingId)
        {
            var booking = await _queryProcessor.ProcessAsync(new BookingIdQuery(bookingId), CancellationToken.None);

            return new BookingResource(Url, booking);
        }
        
        /// <summary>
        /// Add a passenger in booking
        /// </summary>
        /// <param name="bookingId">A unique id for current booking</param>
        /// <param name="addPassengerCommand">Request for adding passenger</param>
        /// <returns></returns>
        [Route("{bookingId}/passenger")]
        [HttpPost]
        public async Task<PassengerAddedResource> AddPassenger(string bookingId,
            [FromBody] AddPassengerCommand addPassengerCommand)
        {
            var command = new Booking.Commands.Passenger.AddPassengerCommand(new BookingId(bookingId))
            {
                Age = addPassengerCommand.Age,
                Email = addPassengerCommand.Email,
                Name = addPassengerCommand.Name,
                PassengerType = addPassengerCommand.PassengerType
            };

            await _commandBus.PublishAsync(command, CancellationToken.None);

            var booking = await _queryProcessor.ProcessAsync(new BookingIdQuery(bookingId), CancellationToken.None);
            var passenger = booking.Passengers.Last();

            return new PassengerAddedResource(Url, bookingId, passenger);
        }

        [Route("{bookingId}/passenger/name")]
        [HttpPost]
        public async Task<PassengerNameUpdatedResource> UpdatePassengerName(string bookingId,
            [FromBody] Resources.Booking.Passenger.Update.UpdatePassengerNameCommand updatePassengerNameCommand)
        {
            var command = new UpdatePassengerNameCommand(new BookingId(bookingId),
                updatePassengerNameCommand.PassengerKey, updatePassengerNameCommand.Name);
            await _commandBus.PublishAsync(command, CancellationToken.None);

            var booking = await _queryProcessor.ProcessAsync(new BookingIdQuery(bookingId), CancellationToken.None);
            var passenger = booking.Passengers.Last();

            return new PassengerNameUpdatedResource(Url, bookingId, passenger);
        }
    }
}