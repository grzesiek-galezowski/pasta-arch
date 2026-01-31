using FunqTypes;
using PastaFit.Core.Domain;

namespace PastaFit.Features.Booking.Ports;

public interface ICreateBookingResponseInProgress
{
  void AlreadyBooked();
  void ClassFull();
  void BookingSuccessfullyCreated(Core.Domain.Booking booking);
  void CouldNotGetClass(Result<Class, BookingError> classResult);
  void MemberInactive();
  void CouldNotFindMember(Result<Member, BookingError> memberResult);
}