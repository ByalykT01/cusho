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
    public async Task<Ok<List<ProductResponseDto>>> GetAllProducts(CancellationToken cancellationToken)
    {
        var result = await productsService.GetAllProductsAsync(cancellationToken);
        return TypedResults.Ok(result.Value);
    }

    [Authorize("IsAdmin")]
    [HttpPost]
    public async Task<Results<CreatedAtRoute<ProductResponseDto>, ProblemHttpResult>> CreateNewProduct(CreateProductDto createProduct, CancellationToken cancellationToken)
    {
        var result = await productsService.CreateProductAsync(createProduct, cancellationToken);
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
     GetProductById(Guid productId, CancellationToken cancellationToken)
    {
        var result = await productsService.GetProductByIdAsync(productId, cancellationToken);

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
     UpdateProduct(Guid productId, UpdateProductDto updateProduct, CancellationToken cancellationToken)
    {
        updateProduct.Id = productId;

        var result = await productsService.UpdateProductByIdAsync(updateProduct, cancellationToken);

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
     DeleteProduct(Guid productId, CancellationToken cancellationToken)
    {
        var result = await productsService.DeleteProductByIdAsync(productId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFoundProblem(result.Error);
        }

        return TypedResults.NoContent();
    }

}
