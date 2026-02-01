using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using PastaFit.Core.Domain;

namespace PastaFit.ComponentTests.Automation;

public class BookingComponentDriver : IAsyncDisposable
{
  private readonly WebApplicationFactory<Program> _factory;
  private readonly Lazy<HttpClient> _client;
  
  public BookingComponentDriver()
  {
    _factory = new WebApplicationFactory<Program>();
    _client = new Lazy<HttpClient>(() => _factory.CreateClient());
  }

  public void Start()
  {
    _ = _client.Value;
  }

  public async ValueTask DisposeAsync()
  {
    await _factory.DisposeAsync();
    await Task.CompletedTask;
  }

  public async Task<RequestGettingClassesTestResponse> RequestGettingClasses()
  {
    return new RequestGettingClassesTestResponse(await _client.Value.GetAsync("/classes"));
  }

  public async Task<GetMembersTestResponse> GetMembers()
  {
    return new GetMembersTestResponse(await _client.Value.GetFromJsonAsync<List<Member>>("/members"));
  }

  public async Task<GetClassesTestResponse> GetClasses()
  {
    return new GetClassesTestResponse(await _client.Value.GetFromJsonAsync<List<ClassAvailability>>("/classes"));
  }

  public async Task<AddBookingTestResponse> RequestAddingBooking(Guid memberId, Guid classId)
  {
    var bookingReq = new
    {
      memberId, classId
    };

    var bookResp = await _client.Value.PostAsJsonAsync("/bookings", (object)bookingReq);
    return new AddBookingTestResponse(bookResp);
  }

  public async Task<AddBookingTestResponse> AddBooking(Guid memberId, Guid classId)
  {
    var addBookingResponse = await RequestAddingBooking(memberId, classId);
    addBookingResponse.ShouldBeSuccessful();
    return addBookingResponse;
  }

  public async Task<DeleteBookingTestResponse> RequestDeletingBooking(Booking? booking)
  {
    return new DeleteBookingTestResponse(await _client.Value.DeleteAsync($"/bookings/{booking!.Id}"));
  }
}