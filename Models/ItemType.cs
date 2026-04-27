using System.ComponentModel.DataAnnotations;

namespace LifecycleManagement.Models;

public class ItemType
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1, 3650)]
    public int DefaultLeadDays { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Item> Items { get; set; } = new List<Item>();
}
