﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Moq;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddUserToRolesWhenUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser invalidUser = null;
            string randomRole = GetRandomString();
            string inputRole = randomRole;

            var nullUserException = new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: nullUserException);

            // when
            ValueTask<ApplicationUser> userTask =
                this.userService.AddUserRolesAsync(invalidUser,inputRole);

            UserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<UserValidationException>(
                  userTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddUserToRoleWhenRoleIsInvalidAndLogItAsync()
        {
            // given
            ApplicationUser user = CreateRandomUser();
            string invalidRole = string.Empty;

            var invalidUserException =
              new InvalidUserException(
                  message: "Invalid User. Please correct the errors and try again.");

            invalidUserException.AddData(
                key: nameof(ApplicationUser),
                values: "Value is required");

            var expectedUserValidationException =
                new UserValidationException(
                    message: "User validation errors occurred, please try again.",
                    innerException: invalidUserException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.userService.AddUserRolesAsync(user,invalidRole);

            UserValidationException actualUserValidationException =
                 await Assert.ThrowsAsync<UserValidationException>(
                     retrieveUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.AddToRoleAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()),
                    Times.Never);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
