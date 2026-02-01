namespace PastaFit.Shell.Endpoints;

public sealed record BookingRequest(Guid MemberId, Guid ClassId);