﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions
{
    public class RideDependencyException : Xeption
    {
        public RideDependencyException(Xeption innerException)
             : base(message: "Ride dependency error occurred, contact support.", innerException) { }
        public RideDependencyException(string message,Xeption innerException)
         : base(message, innerException) { }
    }
}
