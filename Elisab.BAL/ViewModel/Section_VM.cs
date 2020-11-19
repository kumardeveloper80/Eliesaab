using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisab.BAL.ViewModel
{
  public class Section_VM
  {
    public int Id { get; set; }
    public int FashionShowId { get; set; }
    public string Description { get; set; }
    public string MediaType { get; set; }
    public int Sequence { get; set; }
    public List<SectionMedia_VM> sectionMedia { get; set; }

    public Section_VM()
    {
      sectionMedia = new List<SectionMedia_VM>();
    }
  }

  public class SectionMedia_VM
  {
    public int Id { get; set; }
    public int SectionId { get; set; }
    public string MediaName { get; set; }
    public string PosterImageName { get; set; }
  }

  public class Sequence_VM
  {
    public int Id { get; set; }
    public int FashionShowId { get; set; }
    public int Sequence { get; set; }
  }

  public class SequenceList
  {
    public SequenceList()
    {
      sequences = new List<Sequence_VM>();
    }
    public List<Sequence_VM> sequences { get; set; }
  }
}
