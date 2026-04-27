using LifecycleManagement.Models;

namespace LifecycleManagement.Services;

public interface IItemService
{
    int GetEffectiveLeadDays(Item item);
    DateTime GetRenewalStartDate(Item item);
    string GetUrgencyStatus(Item item);
    int GetDaysRemaining(Item item);
    ItemViewModel ToViewModel(Item item);
    Task<List<ItemViewModel>> GetDashboardItemsAsync(string? itemTypeFilter, string? ownerFilter, string? searchTerm, bool myItemsOnly, string? currentUserId);
}
