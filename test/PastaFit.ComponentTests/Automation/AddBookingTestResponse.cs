using System.Net;
using System.Net.Http.Json;
using PastaFit.Core.Domain;

namespace PastaFit.ComponentTests.Automation;

public class AddBookingTestResponse(HttpResponseMessage booking)
{
  private HttpResponseMessage Booking { get; } = booking;

  public void ShouldBeSuccessful()
  {
    Assert.Equal(HttpStatusCode.Created, Booking.StatusCode);
  }

  public async Task<Booking?> ExtractBooking()
  {
    return await Booking.Content.ReadFromJsonAsync<Booking>();
  }
}