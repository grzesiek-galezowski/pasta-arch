using PastaFit.Features.Booking.Adapters;
using PastaFit.Features.Booking.CancelBooking;
using PastaFit.Features.Booking.CreateBooking;
using PastaFit.Features.Booking.Ports;

namespace PastaFit.Shell.Endpoints;

public static class BookingEndpoints
{
}

public sealed record BookingRequest(Guid MemberId, Guid ClassId);