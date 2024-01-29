﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.AdvertAttachments.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jaunts.Core.Api.Tests.Unit.Services.Foundations.AdvertAttachments
{
    public partial class AdvertAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenAdvertIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomAdvertId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputAdvertId = randomAdvertId;

            var invalidAdvertAttachmentException =
              new InvalidAdvertAttachmentException(
                  message: "Invalid AdvertAttachment. Please correct the errors and try again.");

            invalidAdvertAttachmentException.AddData(
                key: nameof(AdvertAttachment.AdvertId),
                values: "Id is required");

            var expectedAdvertAttachmentValidationException =
                new AdvertAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidAdvertAttachmentException);

            // when
            ValueTask<AdvertAttachment> removeAdvertAttachmentTask =
                this.AdvertAttachmentService.RemoveAdvertAttachmentByIdAsync(inputAdvertId, inputAttachmentId);

            AdvertAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AdvertAttachmentValidationException>(
                  removeAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
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
            Guid randomAdvertId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputAdvertId = randomAdvertId;

            var invalidAdvertAttachmentException =
              new InvalidAdvertAttachmentException(
                  message: "Invalid AdvertAttachment. Please correct the errors and try again.");

            invalidAdvertAttachmentException.AddData(
              key: nameof(AdvertAttachment.AttachmentId),
              values: "Id is required");

            var expectedAdvertAttachmentValidationException =
                new AdvertAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    innerException: invalidAdvertAttachmentException);

            // when
            ValueTask<AdvertAttachment> removeAdvertAttachmentTask =
                this.AdvertAttachmentService.RemoveAdvertAttachmentByIdAsync(inputAdvertId, inputAttachmentId);

            AdvertAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AdvertAttachmentValidationException>(
                  removeAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertAttachmentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageAdvertAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            AdvertAttachment randomAdvertAttachment = CreateRandomAdvertAttachment(randomDateTime);
            Guid inputAttachmentId = randomAdvertAttachment.AttachmentId;
            Guid inputAdvertId = randomAdvertAttachment.AdvertId;
            AdvertAttachment nullStorageAdvertAttachment = null;

            var notFoundAdvertAttachmentException =
               new NotFoundAdvertAttachmentException(
                   message: $"Couldn't find attachment with Advert id: {inputAdvertId} " +
                        $"and attachment id: {inputAttachmentId}.");

            var expectedAdvertValidationException =
                new AdvertAttachmentValidationException(
                    message: "Invalid input, contact support.",
                    notFoundAdvertAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectAdvertAttachmentByIdAsync(inputAdvertId, inputAttachmentId))
                    .ReturnsAsync(nullStorageAdvertAttachment);

            // when
            ValueTask<AdvertAttachment> removeAdvertAttachmentTask =
                this.AdvertAttachmentService.RemoveAdvertAttachmentByIdAsync(inputAdvertId, inputAttachmentId);

            AdvertAttachmentValidationException actualAttachmentValidationException =
              await Assert.ThrowsAsync<AdvertAttachmentValidationException>(
                  removeAdvertAttachmentTask.AsTask);

            // then
            actualAttachmentValidationException.Should().BeEquivalentTo(
                expectedAdvertValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAdvertValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAdvertAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAdvertAttachmentAsync(It.IsAny<AdvertAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}