﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class TransactionFeeDependencyValidationException : Xeption
    {
        public TransactionFeeDependencyValidationException(Xeption innerException)
            : base(message: "TransactionFee dependency validation error occurred, fix the errors.", innerException) { }
        public TransactionFeeDependencyValidationException(string message,Xeption innerException)
            : base(message, innerException) { }
    }
}