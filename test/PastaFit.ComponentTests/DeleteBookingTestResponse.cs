using System.Net;

namespace PastaFit.ComponentTests;

public class DeleteBookingTestResponse(HttpResponseMessage deleteBooking)
{
  private HttpResponseMessage DeleteBooking { get; } = deleteBooking;

  public void ShouldBeSuccessful()
  {
    Assert.Equal(HttpStatusCode.NoContent, DeleteBooking.StatusCode);
  }
}