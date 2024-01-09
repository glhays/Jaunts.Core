﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions
{
    public class LockedProvidersDirectorException : Xeption
    {
        public LockedProvidersDirectorException(Exception innerException)
            : base(message: "Locked ProvidersDirector record exception, please try again later.", innerException) { }
        public LockedProvidersDirectorException(string message,Exception innerException)
            : base(message, innerException) { }
    }
}
