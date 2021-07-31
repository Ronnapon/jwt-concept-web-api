using dotnet_hero.DTOs;
using dotnet_hero.Entities;
using dotnet_hero.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace dotnet_hero.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase 
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService) => this.productService = productService;

        [HttpGet] // https://localhost:5050/api/products
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            var product = await productService.FindAll();
            var result =  product.Adapt<IEnumerable<ProductResponse>>();
            return Ok(result);
        }

        [HttpGet("{id}")] // https://localhost:5050/api/products/{id} 
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var product = await productService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }
            var result = product.Adapt<ProductResponse>();
            return Ok(result);
        }

        [HttpGet("search")] // https://localhost:5050/api/products/search?name=imac
        public async Task<ActionResult<IEnumerable<ProductResponse>>> SearchProducts([FromQuery] string name)
        {
            var product = await productService.Search(name);
            var result = product.Adapt<IEnumerable<ProductResponse>>();
            return Ok(result);
        }

        [HttpPost] // https://localhost:5050/api/products 
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductRequest productRequest)
        {
            (string errorMessage, string imageName) = await productService.UploadImage(productRequest.FormFiles);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                return BadRequest();
            }
            var product = productRequest.Adapt<Product>();
            product.Image = imageName;
            await productService.Create(product);
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut("{id}")] // https://localhost:5050/api/products/{id}
        public async Task<ActionResult> UpdateProduct(int id, [FromForm] ProductRequest productRequest)
        {
            if (id != productRequest.ProductId)
            {
                return BadRequest();  
            }

            var product = await productService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }

            (string errorMessage, string imageName) = await productService.UploadImage(productRequest.FormFiles);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                return BadRequest();
            }

            if (!String.IsNullOrEmpty(imageName))
            {
                product.Image = imageName;
            }
            productRequest.Adapt(product);
            await productService.Update(product);
            return NoContent();
        }

        [HttpDelete("{id}")] // https://localhost:5050/api/products/{id}
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await productService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }
            await productService.Delete(product);
            return NoContent();
        }
    }
}
