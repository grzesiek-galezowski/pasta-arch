using PastaFit.Core.Domain;

namespace PastaFit.Features.Booking.Ports;

public interface ICreateBookingResponseInProgress
{
  void AlreadyBooked();
  void ClassFull();
  void BookingSuccessfullyCreated(Core.Domain.Booking booking);
  void CouldNotGetClass(List<BookingError> getClassErrors);
  void MemberInactive();
  void CouldNotFindMember(List<BookingError> memberErrors);
}