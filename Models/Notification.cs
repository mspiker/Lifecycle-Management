using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifecycleManagement.Models;

public class Notification
{
    public int Id { get; set; }

    [Required]
    public int ItemId { get; set; }

    [ForeignKey(nameof(ItemId))]
    public Item Item { get; set; } = null!;

    public DateTime NotificationDate { get; set; }

    public bool SentFlag { get; set; }

    [MaxLength(100)]
    public string? Type { get; set; }
}
