using ASAPTaskAPI.Domain.Entities;
using AutoMapper;

namespace ASAPTaskAPI.Application.Dto
{
    public class ObjectProfiler : Profile
    {
        public ObjectProfiler()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
