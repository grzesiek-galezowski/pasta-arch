using FunqTypes;
using PastaFit.Core.Domain;

namespace PastaFit.Features.Booking.Ports;

public interface ICreateBookingRepository
{
  Task<bool> HasExistingBooking(Guid memberId, Guid classId);
  Task<bool> IsClassFull(Guid classId);
  Task<Result<Member, BookingError>> GetMember(Guid memberId);
  Task<Result<Class, BookingError>> GetClass(Guid classId);
  Task SaveBooking(Core.Domain.Booking booking);
  Task<Result<Core.Domain.Booking, BookingError>> GetBooking(Guid bookingId);
}