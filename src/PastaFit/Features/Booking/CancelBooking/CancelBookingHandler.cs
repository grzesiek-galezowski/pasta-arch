using PastaFit.Features.Booking.Ports;

namespace PastaFit.Features.Booking.CancelBooking;

public static class CancelBookingHandler
{
  public static async Task Handle(Guid bookingId,
    ICancelBookingRepository repository,
    ICancelBookingResponseInProgress responseInProgress)
  {
    var bookingResult = await repository.GetBooking(bookingId);
    if (bookingResult.IsSuccess)
    {
      await repository.CancelBooking(bookingId);
      responseInProgress.Success(bookingResult.Value);
    }
    else
    {
      responseInProgress.Failure(bookingResult.Errors);
    }
  }
}