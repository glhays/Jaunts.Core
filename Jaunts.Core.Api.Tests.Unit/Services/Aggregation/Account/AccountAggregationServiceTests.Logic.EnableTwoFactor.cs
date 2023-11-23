﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Aggregation.Account
{
    public partial class AccountAggregationServiceTests
    {
        [Fact]
        private async Task ShouldEnableTwoFactorAsync()
        {
            // given
            ApplicationUser randomUser = CreateRandomUser();
            ApplicationUser inputUser = randomUser;
            ApplicationUser storageUser = inputUser;
             
            Guid randomId = GetRandomGuid();
            Guid inputId = randomId;

            UserAccountDetailsApiResponse userAccountDetailsResponse =
                CreateUserAccountDetailsApiResponse(inputUser);

            UserAccountDetailsApiResponse expectedUserAccountDetailsResponse =
                    userAccountDetailsResponse;

            this.userOrchestrationMock.Setup(broker =>
                broker.EnableOrDisable2FactorAuthenticationAsync(inputId))
                    .ReturnsAsync(randomUser);

            this.jwtOrchestrationMock.Setup(broker =>
                broker.JwtAccountDetailsAsync(storageUser))
                    .ReturnsAsync(expectedUserAccountDetailsResponse);
            // when
            UserAccountDetailsApiResponse actualAuth =
                await this.accountAggregationService.EnableUser2FARequestAsync(inputId);

            // then
            actualAuth.Should().BeEquivalentTo(expectedUserAccountDetailsResponse);

            this.userOrchestrationMock.Verify(broker =>
                broker.EnableOrDisable2FactorAuthenticationAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.jwtOrchestrationMock.Verify(broker =>
              broker.JwtAccountDetailsAsync(It.IsAny<ApplicationUser>()),
                  Times.Once);

            this.userOrchestrationMock.VerifyNoOtherCalls();
            this.jwtOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
