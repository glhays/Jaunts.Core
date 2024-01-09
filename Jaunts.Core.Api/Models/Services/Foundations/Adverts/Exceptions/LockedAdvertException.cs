﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions
{
    public class LockedAdvertException : Xeption
    {
        public LockedAdvertException(Exception innerException)
            : base(message: "Locked advert record exception, please try again later.", innerException) { }
        public LockedAdvertException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
