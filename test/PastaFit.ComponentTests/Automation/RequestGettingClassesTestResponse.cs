using System.Net;

namespace PastaFit.ComponentTests.Automation;

public class RequestGettingClassesTestResponse(HttpResponseMessage httpResponse)
{
  public void ShouldBeSuccess()
  {
    Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
  }

  public async Task ShouldContain(string expectedString)
  {
    var json = await httpResponse.Content.ReadAsStringAsync();
    Assert.Contains(expectedString, json);
  }
}