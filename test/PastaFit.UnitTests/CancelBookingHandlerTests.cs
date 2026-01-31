using FunqTypes;
using NSubstitute;
using PastaFit.Core.Domain;
using PastaFit.Features.Booking.CancelBooking;
using PastaFit.Features.Booking.Ports;
using TddXt.XNSubstitute;

namespace PastaFit.UnitTests;

public class CancelBookingHandlerTests
{
  [Fact]
  public async Task Fails_If_Booking_Not_Found()
  {
    var repo = Substitute.For<ICancelBookingRepository>();
    var responseInProgress = Substitute.For<ICancelBookingResponseInProgress>();
    var bookingId = Guid.NewGuid();
    var failure = Result<Booking, BookingError>.Fail(new BookingError.BookingNotFound());

    repo.GetBooking(bookingId).Returns(failure);

    await CancelBookingHandler.Handle(bookingId, repo, responseInProgress);

    responseInProgress.ReceivedOnly(1).Failure(failure.Errors);
  }

  [Fact]
  public async Task Succeeds_When_Booking_Exists()
  {
    var bookingId = Guid.NewGuid();
    var booking = new Booking(bookingId, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
    var repo = Substitute.For<ICancelBookingRepository>();
    var responseInProgress = Substitute.For<ICancelBookingResponseInProgress>();

    repo.GetBooking(bookingId).Returns(booking);

    await CancelBookingHandler.Handle(bookingId, repo, responseInProgress);

    XReceived.Exactly(() =>
    {
      repo.CancelBooking(bookingId);
      responseInProgress.Success(booking);
    }, Allowing.Queries());
  }
}