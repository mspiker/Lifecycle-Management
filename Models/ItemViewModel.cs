namespace LifecycleManagement.Models;

public class ItemViewModel
{
    public Item Item { get; set; } = null!;
    public string UrgencyStatus { get; set; } = string.Empty;
    public int DaysRemaining { get; set; }
    public int EffectiveLeadDays { get; set; }
    public DateTime RenewalStartDate { get; set; }
    public string OwnerName { get; set; } = "Unassigned";
}
