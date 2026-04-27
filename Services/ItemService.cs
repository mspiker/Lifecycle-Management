using LifecycleManagement.Data;
using LifecycleManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Services;

public class ItemService : IItemService
{
    private readonly ApplicationDbContext _context;

    public ItemService(ApplicationDbContext context)
    {
        _context = context;
    }

    public int GetEffectiveLeadDays(Item item)
    {
        return item.LeadDaysOverride ?? item.ItemType?.DefaultLeadDays ?? 0;
    }

    public DateTime GetRenewalStartDate(Item item)
    {
        return item.ExpirationDate.AddDays(-GetEffectiveLeadDays(item));
    }

    public string GetUrgencyStatus(Item item)
    {
        var today = DateTime.UtcNow.Date;
        var expiry = item.ExpirationDate.Date;
        var renewalStart = GetRenewalStartDate(item).Date;

        if (today >= expiry)
            return "Critical";
        if (today >= renewalStart)
            return "Approaching";
        return "Safe";
    }

    public int GetDaysRemaining(Item item)
    {
        return (int)(item.ExpirationDate.Date - DateTime.UtcNow.Date).TotalDays;
    }

    public ItemViewModel ToViewModel(Item item)
    {
        return new ItemViewModel
        {
            Item = item,
            UrgencyStatus = GetUrgencyStatus(item),
            DaysRemaining = GetDaysRemaining(item),
            EffectiveLeadDays = GetEffectiveLeadDays(item),
            RenewalStartDate = GetRenewalStartDate(item),
            OwnerName = item.Owner?.DisplayName ?? item.Owner?.Email ?? "Unassigned"
        };
    }

    public async Task<List<ItemViewModel>> GetDashboardItemsAsync(
        string? itemTypeFilter, string? ownerFilter, string? searchTerm,
        bool myItemsOnly, string? currentUserId)
    {
        var query = _context.Items
            .Include(i => i.ItemType)
            .Include(i => i.Owner)
            .Where(i => i.Status == ItemStatus.Active)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
            query = query.Where(i => i.Name.Contains(searchTerm) || (i.Notes != null && i.Notes.Contains(searchTerm)));

        if (!string.IsNullOrEmpty(itemTypeFilter) && int.TryParse(itemTypeFilter, out int typeId))
            query = query.Where(i => i.ItemTypeId == typeId);

        if (myItemsOnly && !string.IsNullOrEmpty(currentUserId))
            query = query.Where(i => i.OwnerUserId == currentUserId);
        else if (!string.IsNullOrEmpty(ownerFilter))
        {
            if (ownerFilter == "unassigned")
                query = query.Where(i => i.OwnerUserId == null);
            else
                query = query.Where(i => i.OwnerUserId == ownerFilter);
        }

        var items = await query.OrderBy(i => i.ExpirationDate).ToListAsync();
        return items.Select(ToViewModel).ToList();
    }
}
