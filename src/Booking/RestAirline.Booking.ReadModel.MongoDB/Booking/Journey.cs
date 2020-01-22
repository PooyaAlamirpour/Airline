﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RestAirline.ReadModel.MongoDb.Booking
{
    public class Journey
    {
        public Guid Id { get; set; }

        public string JourneyKey { get; set; }

        public DateTime DepartureDate { get; set; }

        public string DepartureStation { get; set; }

        public DateTime ArriveDate { get; set; }

        public string ArriveStation { get; set; }

        public string Description { get; set; }

        public Flight Flight { get; set; }
    }

    public static class JourneyMapper
    {
        public static Journey ToReadModel(this RestAirline.Booking.Domain.Booking.Trip.Journey journey)
        {
            var model = new Journey
            {
                Id = Guid.NewGuid(),
                Flight = journey.Flight.ToReadModel(),
                ArriveDate = journey.ArriveDate,
                DepartureDate = journey.DepartureDate,
                ArriveStation = journey.ArriveStation,
                DepartureStation = journey.DepartureStation,
                Description = journey.Description,
                JourneyKey = journey.JourneyKey
            };

            return model;
        }
    }
}