namespace PastaFit.ComponentTests;

public class GetClassesTestResponse(List<ClassAvailability>? classes)
{
  private List<ClassAvailability>? Classes { get; } = classes;

  public ClassAvailability ExtractFirstClass()
  {
    var cls = Classes!.First();
    return cls;
  }
}