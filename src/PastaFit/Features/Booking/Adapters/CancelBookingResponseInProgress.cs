using PastaFit.Core.Domain;
using PastaFit.Features.Booking.Ports;

namespace PastaFit.Features.Booking.Adapters;

public class CancelBookingResponseInProgress
  : ICancelBookingResponseInProgress
{
  public void Success(Core.Domain.Booking bookingResultValue)
  {
    Result = Results.NoContent();
  }

  public void Failure(List<BookingError> bookingResultErrors)
  {
    Result = Results.NotFound();
  }
  
  public IResult Result { get; private set; } = Results.InternalServerError();
}