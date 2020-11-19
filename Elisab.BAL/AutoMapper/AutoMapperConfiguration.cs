using AutoMapper;

namespace Elisab.BAL.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(c =>
            {
                c.AddProfile<AutomapperProfileModelToVM>();
                c.AddProfile<AutomapperProfileVMToModel>();
            });
        }
    }
}
