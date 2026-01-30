using FunqTypes;
using PastaFit.Core.Domain;

namespace PastaFit.Features.Booking.Ports;

public interface ICancelBookingRepository
{
  Task<Result<Core.Domain.Booking, BookingError>> GetBooking(Guid bookingId);
  Task CancelBooking(Guid bookingId);
}