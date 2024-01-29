﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.FlightDeals
{
    public partial class FlightDealServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFlightDealId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFailedFlightDealStorageException =
               new FailedFlightDealStorageException(
                   message: "Failed flightDeal storage error occurred, please contact support.",
                   sqlException);

            var expectedFlightDealDependencyException =
                new FlightDealDependencyException(
                    message: "FlightDeal dependency error occurred, contact support.",
                    expectedFailedFlightDealStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<FlightDeal> retrieveByIdFlightDealTask =
                this.flightDealService.RetrieveFlightDealByIdAsync(someFlightDealId);

            FlightDealDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FlightDealDependencyException>(
                 retrieveByIdFlightDealTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFlightDealDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFlightDealDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFlightDealId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFailedFlightDealStorageException =
              new FailedFlightDealStorageException(
                  message: "Failed flightDeal storage error occurred, please contact support.",
                  databaseUpdateException);

            var expectedFlightDealDependencyException =
                new FlightDealDependencyException(
                    message: "FlightDeal dependency error occurred, contact support.",
                    expectedFailedFlightDealStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<FlightDeal> retrieveByIdFlightDealTask =
                this.flightDealService.RetrieveFlightDealByIdAsync(someFlightDealId);

            FlightDealDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FlightDealDependencyException>(
                 retrieveByIdFlightDealTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFlightDealDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFlightDealId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();


            var lockedFlightDealException = new LockedFlightDealException(
                message: "Locked flightDeal record exception, please try again later.",
                innerException: databaseUpdateConcurrencyException);

            var expectedFlightDealDependencyException =
                new FlightDealDependencyException(
                    message: "FlightDeal dependency error occurred, contact support.",
                    innerException: lockedFlightDealException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<FlightDeal> retrieveByIdFlightDealTask =
                this.flightDealService.RetrieveFlightDealByIdAsync(someFlightDealId);

            FlightDealDependencyException actualDependencyException =
             await Assert.ThrowsAsync<FlightDealDependencyException>(
                 retrieveByIdFlightDealTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(
                expectedFlightDealDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFlightDealId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedFlightDealServiceException =
                 new FailedFlightDealServiceException(
                     message: "Failed flightDeal service error occurred, contact support.",
                     innerException: serviceException);

            var expectedFlightDealServiceException =
                new FlightDealServiceException(
                    message: "FlightDeal service error occurred, contact support.",
                    innerException: failedFlightDealServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<FlightDeal> retrieveByIdFlightDealTask =
                this.flightDealService.RetrieveFlightDealByIdAsync(someFlightDealId);

            FlightDealServiceException actualServiceException =
                 await Assert.ThrowsAsync<FlightDealServiceException>(
                     retrieveByIdFlightDealTask.AsTask);

            // then
            actualServiceException.Should().BeEquivalentTo(
                expectedFlightDealServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFlightDealByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFlightDealServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
