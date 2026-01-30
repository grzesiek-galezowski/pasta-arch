using FunqTypes;
using NSubstitute;
using PastaFit.Core.Domain;
using PastaFit.Features.Booking.CancelBooking;
using PastaFit.Features.Booking.Ports;

namespace PastaFit.UnitTests;

public class CancelBookingHandlerTests
{
    [Fact]
    public async Task Fails_If_Booking_Not_Found()
    {
        var repo = Substitute.For<ICancelBookingRepository>();
        var bookingId = Guid.NewGuid();
        
        repo.GetBooking(bookingId).Returns(Result<Booking, BookingError>.Fail(new BookingError.BookingNotFound()));

        var result = await CancelBookingHandler.Handle(bookingId, repo);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e is BookingError.BookingNotFound);
    }

    [Fact]
    public async Task Succeeds_When_Booking_Exists()
    {
        var bookingId = Guid.NewGuid();
        var booking = new Booking(bookingId, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        var repo = Substitute.For<ICancelBookingRepository>();

        repo.GetBooking(bookingId).Returns(booking);

        var result = await CancelBookingHandler.Handle(bookingId, repo);

        Assert.True(result.IsSuccess);
        Assert.Equal(bookingId, result.Value.Id);
        await repo.Received(1).CancelBooking(bookingId);
  }
}