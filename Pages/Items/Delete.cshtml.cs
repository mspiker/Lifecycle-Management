using LifecycleManagement.Data;
using LifecycleManagement.Models;
using LifecycleManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Pages.Items;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IItemService _itemService;

    public DeleteModel(ApplicationDbContext context, IItemService itemService)
    {
        _context = context;
        _itemService = itemService;
    }

    [BindProperty]
    public Item Item { get; set; } = default!;

    public ItemViewModel ItemVM { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.Items
            .Include(i => i.ItemType)
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (item == null) return NotFound();

        Item = item;
        ItemVM = _itemService.ToViewModel(item);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.Items.FindAsync(id);
        if (item != null)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
