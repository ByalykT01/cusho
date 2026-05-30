using cusho.Data;
using cusho.Dtos.ProductDtos;
using cusho.Infrastructure;
using cusho.Models;
using Microsoft.EntityFrameworkCore;

namespace cusho.Services;

public class ProductsService(ApplicationDbContext dbContext, ILogger<ProductsService> logger)
{
    public async Task<Result<List<ProductResponseDto>>> GetAllProductsAsync()
    {
        var allProducts = await dbContext.Products.AsNoTracking().Select(p => new ProductResponseDto()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price
        }).ToListAsync();

        return allProducts;
    }

    public async Task<Result<ProductResponseDto>> CreateProductAsync(CreateProductDto createProductDto)
    {
        var createdProduct = new Product()
        {
            Id = Guid.NewGuid(),
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price
        };
        await dbContext.Products.AddAsync(createdProduct);

        await dbContext.SaveChangesAsync();
        var productResponse = new ProductResponseDto
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price
        };
        return productResponse;
    }

    public async Task<Result<ProductResponseDto>> GetProductByIdAsync(Guid productId)
    {
        var foundProduct = await dbContext.Products.AsNoTracking().Where(p => p.Id.Equals(productId)).Select(p => new ProductResponseDto()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price
        }).FirstOrDefaultAsync();

        return foundProduct ?? Result<ProductResponseDto>.Failure("Product not found");
    }

    public async Task<Result<ProductResponseDto>> UpdateProductByIdAsync(UpdateProductDto updateProduct)
    {
        var foundProduct = await dbContext.Products.FindAsync(updateProduct.Id);

        if (foundProduct is null)
            return Result<ProductResponseDto>.Failure("Product not found");

        foundProduct.Name = updateProduct.Name;
        foundProduct.Description = updateProduct.Description;
        foundProduct.Price = updateProduct.Price;
        foundProduct.IsAvailable = updateProduct.IsAvailable;

        if (updateProduct.CategoryId.HasValue)
            foundProduct.CategoryId = updateProduct.CategoryId.Value;

        if (updateProduct.CollectionId.HasValue)
            foundProduct.CollectionId = updateProduct.CollectionId.Value;

        await dbContext.SaveChangesAsync();

        return new ProductResponseDto
        {
            Id = foundProduct.Id,
            Name = foundProduct.Name,
            Description = foundProduct.Description,
            Price = foundProduct.Price
        };
    }

    public async Task<Result<bool>> DeleteProductByIdAsync(Guid productId)
    {
        var foundProduct = await dbContext.Products.FindAsync(productId);

        if (foundProduct is null)
            return Result<bool>.Failure("Product not found");

        dbContext.Products.Remove(foundProduct);
        await dbContext.SaveChangesAsync();

        return true;
    }
}
