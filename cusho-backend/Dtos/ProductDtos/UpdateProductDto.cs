using System.ComponentModel.DataAnnotations;

namespace cusho.Dtos.ProductDtos;

public class UpdateProductDto
{
    public required Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(1, double.MaxValue, ErrorMessage = "Price must be at least 1.00")]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? CollectionId { get; set; }

    public List<string>? ImageUrls { get; set; }

    public List<Guid>? TagIds { get; set; }
}
