﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.User.Exceptions
{
    public class UserProcessingServiceException : Xeption
    {
        public UserProcessingServiceException(Exception innerException)
            : base(message: "Failed external User service occurred, please contact support", innerException)
        { }
    }
}