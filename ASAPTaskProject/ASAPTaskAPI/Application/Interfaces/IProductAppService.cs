using ASAPTaskAPI.Application.Dto;
using ASAPTaskAPI.Domain.Entities;
using ASAPTaskAPI.Helper;

namespace ASAPTaskAPI.Application.Interfaces
{
    public interface IProductAppService
    {
        Task Create(Product product);
        Task Delete(int id);
        Task<PaginatedResult<Product>> GetAll(GetProductInputDto input);
        Task<Product?> GetById(int id);
        Task Update(Product product);
    }
}
