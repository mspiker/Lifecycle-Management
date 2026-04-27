using LifecycleManagement.Data;
using LifecycleManagement.Models;
using LifecycleManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IItemService _itemService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(IItemService itemService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _itemService = itemService;
        _context = context;
        _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ItemTypeFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? OwnerFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool MyItemsOnly { get; set; }

    public List<ItemViewModel> AllItems { get; set; } = new();
    public List<ItemViewModel> CriticalItems { get; set; } = new();
    public List<ItemViewModel> ApproachingItems { get; set; } = new();
    public List<ItemViewModel> UpcomingItems { get; set; } = new();
    public List<ItemType> ItemTypes { get; set; } = new();
    public List<ApplicationUser> Users { get; set; } = new();
    public string? CurrentUserId { get; set; }

    public async Task OnGetAsync()
    {
        CurrentUserId = _userManager.GetUserId(User);
        ItemTypes = await _context.ItemTypes.Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync();
        Users = await _userManager.Users.OrderBy(u => u.DisplayName ?? u.Email).ToListAsync();

        AllItems = await _itemService.GetDashboardItemsAsync(ItemTypeFilter, OwnerFilter, SearchTerm, MyItemsOnly, CurrentUserId);

        CriticalItems = AllItems.Where(i => i.UrgencyStatus == "Critical").ToList();
        ApproachingItems = AllItems.Where(i => i.UrgencyStatus == "Approaching").ToList();
        UpcomingItems = AllItems.Where(i => i.UrgencyStatus == "Safe" && i.DaysRemaining <= 30).ToList();
    }
}
