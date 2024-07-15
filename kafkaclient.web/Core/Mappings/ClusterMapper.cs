using AutoMapper;
using kafkaclient.web.Core.Dto;
using kafkaclient.web.Core.Entities;

namespace kafkaclient.web.Core.Mappings;

public class ClusterMapper : Profile
{
    public ClusterMapper()
    {
        CreateMap<ClusterDto, Cluster>()
            .ReverseMap()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<ClusterRequest, Cluster>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == "Activo" ? true : false));
    }
}