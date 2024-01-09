﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions
{
    public class ProviderServiceDependencyValidationException : Xeption
    {
        public ProviderServiceDependencyValidationException(Xeption innerException)
            : base(message: "ProviderService dependency validation error occurred, fix the errors.", innerException) { }
        public ProviderServiceDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}
