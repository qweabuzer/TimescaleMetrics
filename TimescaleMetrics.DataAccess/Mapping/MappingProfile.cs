using AutoMapper;
using TimescaleMetrics.Core.Models;
using TimescaleMetrics.DataAccess.Entities;

namespace TimescaleMetrics.DataAccess.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ValueEntity, CSValue>()
                .ConstructUsing(src => CSValue.Create(
                    src.Id,
                    src.Date,
                    src.ExecutionTime,
                    src.Value).Value)
                .ReverseMap();

            CreateMap<IntegrationResults, ResultEntity>()
                .ForMember(r => r.Id, opt => opt.Ignore())
                .ForMember(r => r.FileName, opt => opt.Ignore())
                .ReverseMap();

        }
    }
}
