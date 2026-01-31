using FunqTypes;
using PastaFit.Core.Domain;
using PastaFit.Features.Booking.Ports;

namespace PastaFit.Features.Booking.Adapters;

public class CreateBookingResponseInProgress
  : ICreateBookingResponseInProgress
{
  public IResult Result { get; private set; } = Results.InternalServerError();

  public void AlreadyBooked()
  {
    Result = Results.BadRequest(new BookingError.AlreadyBooked()); //bug use asp.net core Result instead
  }

  public void ClassFull()
  {
    Result = Results.BadRequest(new BookingError.ClassFull());
  }

  public void BookingSuccessfullyCreated(Core.Domain.Booking booking)
  {
    Result = Results.Created($"/bookings/{booking.Id}", booking);
  }

  public void CouldNotGetClass(Result<Class, BookingError> classResult)
  {
    Result = Results.BadRequest(classResult.Errors.ToArray());
  }

  public void MemberInactive()
  {
    Result = Results.BadRequest(new BookingError.MemberInactive());
  }

  public void CouldNotFindMember(Result<Member, BookingError> memberResult)
  {
    Result = Results.BadRequest(memberResult.Errors.ToArray());
  }
}