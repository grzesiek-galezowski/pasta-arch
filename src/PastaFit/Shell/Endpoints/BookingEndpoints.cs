using PastaFit.Features.Booking.Adapters;
using PastaFit.Features.Booking.CancelBooking;
using PastaFit.Features.Booking.CreateBooking;
using PastaFit.Features.Booking.Ports;

namespace PastaFit.Shell.Endpoints;

public static class BookingEndpoints
{
  public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder app)
  {
    app.MapPost(
      "/bookings",
      async (
        BookingRequest request,
        ICreateBookingRepository deps) =>
      {
        var responseInProgress = new CreateBookingResponseInProgress();
        await CreateBookingHandler.Handle(request.MemberId, request.ClassId, deps, responseInProgress);
        return responseInProgress.Result;
      });

    app.MapDelete(
      "/bookings/{bookingId:guid}",
      async (
        Guid bookingId,
        ICancelBookingRepository deps) =>
      {
        var responseInProgress = new CancelBookingResponseInProgress();
        await CancelBookingHandler.Handle(bookingId, deps, responseInProgress);
        return responseInProgress.Result;
      });

    app.MapGet("/classes", InMemoryBookingRepository.GetClassAvailability);

    app.MapGet("/members", InMemoryBookingRepository.GetAllMembers);

    return app;
  }
}

public sealed record BookingRequest(Guid MemberId, Guid ClassId);