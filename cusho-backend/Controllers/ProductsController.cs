using cusho.Dtos.ProductDtos;
using cusho.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cusho.Controllers;

[Route("api/[controller]")]
public sealed class ProductsController(ProductsService productsService) : ApiControllerBase
{
    [HttpGet]
    public async Task<Ok<List<ProductResponseDto>>> GetAllProducts()
    {
        var result = await productsService.GetAllProductsAsync();
        return TypedResults.Ok(result.Value);
    }

    [Authorize("IsAdmin")]
    [HttpPost]
    public async Task<Results<CreatedAtRoute<ProductResponseDto>, ProblemHttpResult>> CreateNewProduct(CreateProductDto createProduct)
    {
        var result = await productsService.CreateProductAsync(createProduct);
        if (result.IsFailure)
        {
            return BadRequestProblem(result.Error);
        }

        return TypedResults.CreatedAtRoute(result.Value,
                nameof(GetProductById),
                new { productId = result.Value.Id });
    }

    [HttpGet("{productId}", Name = nameof(GetProductById))]
    public async Task<Results<
        Ok<ProductResponseDto>,
        ProblemHttpResult>>
     GetProductById(Guid productId)
    {
        var result = await productsService.GetProductByIdAsync(productId);

        if (result.IsFailure)
        {
            return NotFoundProblem(result.Error);
        }

        return TypedResults.Ok(result.Value);
    }

    [Authorize("IsAdmin")]
    [HttpPut("{productId}")]
    public async Task<Results<
        Ok<ProductResponseDto>,
        ProblemHttpResult>>
     UpdateProduct(Guid productId, UpdateProductDto updateProduct)
    {
        updateProduct.Id = productId;

        var result = await productsService.UpdateProductByIdAsync(updateProduct);

        if (result.IsFailure)
        {
            return NotFoundProblem(result.Error);
        }

        return TypedResults.Ok(result.Value);
    }

    [Authorize("IsAdmin")]
    [HttpDelete("{productId}")]
    public async Task<Results<
        NoContent,
        ProblemHttpResult>>
     DeleteProduct(Guid productId)
    {
        var result = await productsService.DeleteProductByIdAsync(productId);

        if (result.IsFailure)
        {
            return NotFoundProblem(result.Error);
        }

        return TypedResults.NoContent();
    }

}
