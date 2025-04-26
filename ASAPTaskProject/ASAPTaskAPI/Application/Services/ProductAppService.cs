using ASAPTaskAPI.Application.Dto;
using ASAPTaskAPI.Application.Interfaces;
using ASAPTaskAPI.Domain.Entities;
using ASAPTaskAPI.Helper;
using ASAPTaskAPI.Infrastructure.Data;
using ASAPTaskAPI.Infrastructure.Interface;
using AutoMapper;
using System;

namespace ASAPTaskAPI.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IMapper _mapper;

        private readonly IRepository<Product> _productRepository;

        public ProductAppService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;

        }

        public async Task Create(Product product)
        {
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                _productRepository.Delete(product);
                await _productRepository.SaveChangesAsync();
            }
        }

        public async Task<PaginatedResult<Product>> GetAll(GetProductInputDto input)
        {
            var query = (await _productRepository.GetAllAsync()).AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.Search))
            {
                query = query.Where(p =>
                    p.Name.Contains(input.Search) ||
                    p.Price.ToString().Contains(input.Search)
                );
            }

            var totalCount = query.Count();

            var products = query
                .Skip((input.Page - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            return new PaginatedResult<Product>
            {
                Items = products,
                TotalCount = totalCount
            };
        }

        public async Task<Product?> GetById(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task Update(Product product)
        {
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }
    }
}
