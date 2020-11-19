using AutoMapper;
using Elisab.BAL.ViewModel;
using Elisab.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elisab.BAL.Repository
{
  public class SectionRepository
  {
    ElisabDbContext _context;

    public SectionRepository()
    {
      _context = new ElisabDbContext();
    }

    public List<Section_VM> GetByFashionShowId(int Id)
    {
      var result = _context.Sections.Where(x => x.FashionShowId == Id && x.DeletedDate == null).OrderBy(x => x.Sequence).ToList();
      if (result != null)
      {
        return Mapper.Map<List<Section_VM>>(result);
      }
      return null;
    }

    public int Delete(int Id, int DeletedBy)
    {
      var result = _context.Sections.Where(x => x.Id == Id).FirstOrDefault();
      if (result != null)
      {
        var sequenceList = _context.Sections.Where(x => x.FashionShowId == result.FashionShowId && x.Sequence > result.Sequence && x.DeletedDate == null).OrderBy(x => x.Sequence).ToList();
        if (sequenceList.Any())
        {
          foreach (var section in sequenceList)
          {
            section.Sequence = section.Sequence - 1;
            _context.SaveChanges();
          }
        }


        result.DeletedDate = DateTime.Now;
        result.DeletedBy = DeletedBy;
        return _context.SaveChanges();
      }
      return 0;
    }

    public Section_VM GetById(int Id)
    {
      var result = (from s in _context.Sections
                    where s.Id == Id && s.DeletedDate == null
                    select new Section_VM
                    {
                      Id = s.Id,
                      MediaType = s.MediaType,
                      Description = s.Description,
                      sectionMedia = (from sm in _context.SectionMedias
                                      where sm.SectionId == Id
                                      select new SectionMedia_VM
                                      {
                                        Id = sm.Id,
                                        MediaName = sm.MediaName,
                                        PosterImageName = sm.PosterImageName
                                      }).ToList()
                    }).FirstOrDefault();

      return Mapper.Map<Section_VM>(result);
    }

    public void UpdateSequnce(List<Sequence_VM> sequences)
    {
      foreach (var seq in sequences)
      {
        var section = _context.Sections.Where(x => x.Id == seq.Id && x.DeletedDate == null).FirstOrDefault();
        if (section != null)
        {
          section.Sequence = seq.Sequence + 1;
          _context.SaveChanges();
        }
      }
    }

    public int AddSection(int CreatedBy, Section_VM section_VM)
    {
      var section = Mapper.Map<Section>(section_VM);
      section.CreatedBy = CreatedBy;
      section.CreatedDate = DateTime.Now;
      section = _context.Sections.Add(section);
      _context.SaveChanges();
      return section.Id;
    }

    public int UpdateSection(int Id, int UpdatedBy, Section_VM section_VM)
    {
      var old = _context.Sections.Where(x => x.Id == Id).FirstOrDefault();
      if (old != null)
      {
        old.FashionShowId = section_VM.FashionShowId;
        old.MediaType = section_VM.MediaType;
        old.Description = section_VM.Description;
        old.UpdatedBy = UpdatedBy;
        old.UpdatedDate = DateTime.Now;
        return _context.SaveChanges();
      }
      return 0;
    }


    public int UpdateSectionMedia(int Id, SectionMedia_VM sectionMedia_VM)
    {
      var old = _context.SectionMedias.Where(x => x.Id == Id).FirstOrDefault();
      if (old != null)
      {
        old.SectionId = sectionMedia_VM.SectionId;
        old.MediaName = sectionMedia_VM.MediaName;
        old.PosterImageName = sectionMedia_VM.PosterImageName;
        return _context.SaveChanges();
      }
      return 0;
    }

    public int AddSectionMedia(SectionMedia_VM sectionMedia_VM)
    {
      var sectionMedia = Mapper.Map<SectionMedia>(sectionMedia_VM);
      _context.SectionMedias.Add(sectionMedia);
      return _context.SaveChanges();
    }
  }
}
