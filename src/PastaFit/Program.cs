using PastaFit.Features.Booking.Adapters;
using PastaFit.Features.Booking.CancelBooking;
using PastaFit.Features.Booking.CreateBooking;
using PastaFit.Shell.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var repository = new InMemoryBookingRepository();
repository.Bootstrap();

var app = builder.Build();

app.MapPost(
  "/bookings",
  async (
    BookingRequest request) =>
  {
    var responseInProgress = new CreateBookingResponseInProgress();
    await CreateBookingHandler.Handle(request.MemberId, request.ClassId, repository, responseInProgress);
    return responseInProgress.Result;
  });

app.MapDelete(
  "/bookings/{bookingId:guid}",
  async (
    Guid bookingId) =>
  {
    var responseInProgress = new CancelBookingResponseInProgress();
    await CancelBookingHandler.Handle(bookingId, repository, responseInProgress);
    return responseInProgress.Result;
  });

app.MapGet("/classes", InMemoryBookingRepository.GetClassAvailability);

app.MapGet("/members", InMemoryBookingRepository.GetAllMembers);

app.Run();

public partial class Program { }
