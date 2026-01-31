using Microsoft.AspNetCore.Mvc.Testing;

namespace PastaFit.ComponentTests;

public class BookingComponentTests : IClassFixture<WebApplicationFactory<Program>>
{
  [Fact]
  public async Task Can_Get_Classes()
  {
    var driver = new BookingComponentDriver();
    driver.Start();

    var testResponse = await driver.RequestGettingClasses();

    testResponse.ShouldBeSuccess();
    await testResponse.ShouldContain("Yoga");
  }

  [Fact]
  public async Task Can_Book_And_Cancel_Class()
  {
    var driver = new BookingComponentDriver();
    driver.Start();
    
    var membersResponse = await driver.GetMembers();
    var getClassesResponse = await driver.GetClasses();
    var addBookingResponse = await driver.AddBooking(
      membersResponse.ExtractFirstActiveMember().Id,
      getClassesResponse.ExtractFirstClass().Id);
    var booking = await addBookingResponse.ExtractBooking();
    
    var deleteBookingResponse = await driver.RequestDeletingBooking(booking);

    deleteBookingResponse.ShouldBeSuccessful();
  }
}

public record ClassAvailability(Guid Id, string Name, int Capacity, int Available);
