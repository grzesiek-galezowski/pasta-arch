using PastaFit.Core.Domain;

namespace PastaFit.ComponentTests.Automation;

public class GetMembersTestResponse(List<Member>? members)
{
  private List<Member>? Members { get; } = members;

  public Member ExtractFirstActiveMember()
  {
    return Members!.First(m => m.IsActive);
  }
}