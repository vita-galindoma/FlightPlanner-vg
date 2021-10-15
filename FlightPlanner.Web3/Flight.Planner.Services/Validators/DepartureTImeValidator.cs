﻿using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace Flight.Planner.Services.Validators
{
    public class DepartureTImeValidator : IValidator
    {
        public bool IsValid(FlightRequest request)
        {
            return !string.IsNullOrEmpty(request.DepartureTime);
        }
    }
}
