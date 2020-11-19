using System.Collections.Generic;

namespace Elisab.BAL.ViewModel
{
  public class LandingPage_VM
  {
    public int fashionId { get; set; }
    public bool IsFutureEvent { get; set; }
    public MainPage_VM mainPage { get; set; }
    public SecondPage_VM secondPage { get; set; }
    public Address_VM addressPage { get; set; }
    public List<Section_VM> sectionPage { get; set; }

    public LandingPage_VM()
    {
      mainPage = new MainPage_VM();
      secondPage = new SecondPage_VM();
      addressPage = new Address_VM();
      sectionPage = new List<Section_VM>();
    }
  }
}
