using System.ComponentModel.DataAnnotations;

namespace Cw6.Models.DTOs;

public class AddAnimalRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    [Required]
    [MaxLength(200)]
    public string? Description { get; set; }
}