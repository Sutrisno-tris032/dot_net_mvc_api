using Microsoft.AspNetCore.Mvc;
using MvcApi.Interface;
using MvcApi.Models;

namespace MvcApi.Controllers;

[Route("Product")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductInterface _products;

    public ProductsController(ProductInterface products) => _products = products;

    [HttpGet]
    [Route("/")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetAll()
    {
        return await _products.GetProductList();
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<ProductModel>> GetById(int id)
    {
        var product = await _products.GetByIdAsync(id);
        return product is null ? NotFound() : product;
    }

    [HttpPost]
    [Route("/")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<ProductModel>> Create(ProductModel input)
    {
        var created = await _products.AddProductAsync(input);
        return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
    }

    [HttpPut]
    [Route("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(int id, ProductModel input)
    {
        //if (id != input.id) return BadRequest("Id mismatch.");

        var updated = await _products.UpdateProductAsync(id, input);
        return updated is null ? NotFound() : NoContent();
    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<ProductModel>> Delete(int id)
    {
        var deleted = await _products.DeleteProductAsync(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
