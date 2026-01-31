using PastaFit.Core.Domain;

namespace PastaFit.Features.Booking.Ports;

public interface ICancelBookingResponseInProgress
{
  void Success(Core.Domain.Booking bookingResultValue);
  void Failure(List<BookingError> bookingResultErrors);
}