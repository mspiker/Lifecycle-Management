using LifecycleManagement.Data;
using LifecycleManagement.Models;
using LifecycleManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Pages.Items;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IItemService _itemService;

    public DetailsModel(ApplicationDbContext context, IItemService itemService)
    {
        _context = context;
        _itemService = itemService;
    }

    public ItemViewModel ItemVM { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.Items
            .Include(i => i.ItemType)
            .Include(i => i.Owner)
            .Include(i => i.Notifications)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (item == null) return NotFound();

        ItemVM = _itemService.ToViewModel(item);
        return Page();
    }
}
