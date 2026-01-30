using PastaFit.Features.Booking.Adapters;
using PastaFit.Features.Booking.CancelBooking;
using PastaFit.Features.Booking.CreateBooking;
using PastaFit.Features.Booking.Ports;

namespace PastaFit.Shell.Endpoints;

public static class BookingEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/bookings", async (
            BookingRequest request,
            ICreateBookingRepository deps) =>
        {
            var result = await CreateBookingHandler.Handle(request.MemberId, request.ClassId, deps);
            return result.Match(
                booking => Results.Created($"/bookings/{booking.Id}", booking),
                Results.BadRequest
            );
        });
        
        app.MapDelete("/bookings/{bookingId:guid}", async (
            Guid bookingId,
            ICancelBookingRepository deps) =>
        {
            var result = await CancelBookingHandler.Handle(bookingId, deps);
            return result.Match(
                _ => Results.NoContent(),
                Results.NotFound
            );
        });

        app.MapGet("/classes", InMemoryBookingRepository.GetClassAvailability);
        
        app.MapGet("/members", InMemoryBookingRepository.GetAllMembers);

        return app;
    }
}

public sealed record BookingRequest(Guid MemberId, Guid ClassId);