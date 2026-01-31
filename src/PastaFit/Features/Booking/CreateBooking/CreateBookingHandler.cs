using PastaFit.Features.Booking.Ports;

namespace PastaFit.Features.Booking.CreateBooking;

public static class CreateBookingHandler
{
  public static async Task Handle(
    Guid memberId,
    Guid classId,
    ICreateBookingRepository repo,
    ICreateBookingResponseInProgress response)
  {
    var memberResult = await repo.GetMember(memberId);
    if (memberResult.IsSuccess)
    {
      if (memberResult.Value.IsActive)
      {
        var classResult = await repo.GetClass(classId);
        if (classResult.IsSuccess)
        {
          if (await repo.HasExistingBooking(memberId, classId))
          {
            response.AlreadyBooked();
          }
          else if (await repo.IsClassFull(classId))
          {
            response.ClassFull();
          }
          else
          {
            var booking = new Core.Domain.Booking(Guid.NewGuid(), memberId, classId, DateTime.UtcNow);
            await repo.SaveBooking(booking);
            
            response.BookingSuccessfullyCreated(booking);
          }
        }
        else
        {
          response.CouldNotGetClass(classResult.Errors);
        }
      }
      else
      {
        response.MemberInactive();
      }
    }
    else
    {
      response.CouldNotFindMember(memberResult.Errors);
    }
  }
}