using LifecycleManagement.Data;
using LifecycleManagement.Models;
using LifecycleManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Pages.Items;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IItemService _itemService;

    public IndexModel(ApplicationDbContext context, IItemService itemService)
    {
        _context = context;
        _itemService = itemService;
    }

    public List<ItemViewModel> Items { get; set; } = new();

    public async Task OnGetAsync()
    {
        var items = await _context.Items
            .Include(i => i.ItemType)
            .Include(i => i.Owner)
            .OrderBy(i => i.ExpirationDate)
            .ToListAsync();

        Items = items.Select(_itemService.ToViewModel).ToList();
    }
}
