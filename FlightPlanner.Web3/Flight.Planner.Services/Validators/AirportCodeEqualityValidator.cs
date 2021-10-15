﻿using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace Flight.Planner.Services.Validators
{
    public class AirportCodeEqualityValidator : IValidator
    {
        public bool IsValid(FlightRequest request)
        {
            return request?.From?.Airport?.Trim().ToLower() != request?.To?.Airport?.Trim().ToLower();
        }
    }
}
