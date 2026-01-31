using PastaFit.Core.Domain;
using PastaFit.Features.Booking.Ports;

namespace PastaFit.Features.Booking.Adapters;

public class CreateBookingResponseInProgress : ICreateBookingResponseInProgress
{
  public IResult Result { get; private set; } = Results.InternalServerError();

  public void AlreadyBooked()
  {
    Result = Results.BadRequest(new BookingError.AlreadyBooked());
  }

  public void ClassFull()
  {
    Result = Results.BadRequest(new BookingError.ClassFull());
  }

  public void BookingSuccessfullyCreated(Core.Domain.Booking booking)
  {
    Result = Results.Created($"/bookings/{booking.Id}", booking);
  }

  public void CouldNotGetClass(List<BookingError> getClassErrors)
  {
    Result = Results.BadRequest(getClassErrors.ToArray());
  }

  public void MemberInactive()
  {
    Result = Results.BadRequest(new BookingError.MemberInactive());
  }

  public void CouldNotFindMember(List<BookingError> memberErrors)
  {
    Result = Results.BadRequest(memberErrors.ToArray());
  }
}