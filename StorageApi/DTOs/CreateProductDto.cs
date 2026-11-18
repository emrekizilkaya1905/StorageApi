
using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs;

public class CreateProductDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Price { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? Shelf { get; set; }

    [Range(0, int.MaxValue)]
    public int Count { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }
}