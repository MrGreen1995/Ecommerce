using System.Net;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

public class CatalogController : ApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("[action]/{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProductById(string id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("[action]/{productName}", Name = "GetProductByProductName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductByProductName(string productName)
    {
        var query = new GetProductByNameQuery(productName);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("[action]/{brandName}", Name = "GetProductByBrandName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductByBrandName(string brandName)
    {
        var query = new GetProductByBrandQuery(brandName);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("GetAllProducts")]
    [ProducesResponseType(typeof(IList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts()
    {
        var query = new GetAllProductsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("GetAllBrands")]
    [ProducesResponseType(typeof(IList<BrandResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBrands()
    {
        var query = new GetAllBrandsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("GetAllTypes")]
    [ProducesResponseType(typeof(IList<TypeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTypes()
    {
        var query = new GetAllTypesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("CreateProduct")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand productCommand)
    {
        var result = await _mediator.Send(productCommand);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("UpdateProduct")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand productCommand)
    {
        var result = await _mediator.Send(productCommand);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        var command = new DeleteProductByIdCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}