﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.PackageAttachments
{
    public partial class PackageAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenPackageIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomPackageId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputPackageId = randomPackageId;

            var invalidPackageAttachmentException =
              new InvalidPackageAttachmentException(
                  message: "Invalid PackageAttachment. Please correct the errors and try again.");

            invalidPackageAttachmentException.AddData(
                key: nameof(PackageAttachment.PackageId),
                values: "Id is required");

            var expectedPackageAttachmentValidationException =
                new PackageAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidPackageAttachmentException);

            // when
            ValueTask<PackageAttachment> removePackageAttachmentTask =
                this.packageAttachmentService.RemovePackageAttachmentByIdAsync(inputPackageId, inputAttachmentId);

            PackageAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PackageAttachmentValidationException>(
                  removePackageAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomPackageId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputPackageId = randomPackageId;

            var invalidPackageAttachmentException =
              new InvalidPackageAttachmentException(
                  message: "Invalid PackageAttachment. Please correct the errors and try again.");

            invalidPackageAttachmentException.AddData(
              key: nameof(PackageAttachment.AttachmentId),
              values: "Id is required");

            var expectedPackageAttachmentValidationException =
                new PackageAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidPackageAttachmentException);

            // when
            ValueTask<PackageAttachment> removePackageAttachmentTask =
                this.packageAttachmentService.RemovePackageAttachmentByIdAsync(inputPackageId, inputAttachmentId);

            PackageAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PackageAttachmentValidationException>(
                  removePackageAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStoragePackageAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            PackageAttachment randomPackageAttachment = CreateRandomPackageAttachment(randomDateTime);
            Guid inputAttachmentId = randomPackageAttachment.AttachmentId;
            Guid inputPackageId = randomPackageAttachment.PackageId;
            PackageAttachment nullStoragePackageAttachment = null;

            var notFoundPackageAttachmentException =
               new NotFoundPackageAttachmentException(
                   message: $"Couldn't find attachment with Package id: {inputPackageId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedPackageValidationException =
                new PackageAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundPackageAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectPackageAttachmentByIdAsync(inputPackageId, inputAttachmentId))
                    .ReturnsAsync(nullStoragePackageAttachment);

            // when
            ValueTask<PackageAttachment> removePackageAttachmentTask =
                this.packageAttachmentService.RemovePackageAttachmentByIdAsync(inputPackageId, inputAttachmentId);

            PackageAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<PackageAttachmentValidationException>(
                  removePackageAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedPackageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPackageValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPackageAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePackageAttachmentAsync(It.IsAny<PackageAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}