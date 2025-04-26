using ASAPTaskAPI.Application.Dto;
using ASAPTaskAPI.Application.Interfaces;
using ASAPTaskAPI.Domain.Entities;
using ASAPTaskAPI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASAPTaskAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductAppService _productService;

        public ProductsController(IProductAppService productService) => _productService = productService;

        [HttpGet]
        public IActionResult Get([FromQuery]GetProductInputDto input) => Ok(_productService.GetAll(input));

        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            _productService.Create(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();
            _productService.Update(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return NoContent();
        }
    }

}
