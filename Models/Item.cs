using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifecycleManagement.Models;

public class Item
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int ItemTypeId { get; set; }

    [ForeignKey(nameof(ItemTypeId))]
    public ItemType ItemType { get; set; } = null!;

    [Required]
    public DateTime ExpirationDate { get; set; }

    public int? LeadDaysOverride { get; set; }

    public string? OwnerUserId { get; set; }

    [ForeignKey(nameof(OwnerUserId))]
    public ApplicationUser? Owner { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public ItemStatus Status { get; set; } = ItemStatus.Active;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
