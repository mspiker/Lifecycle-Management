using LifecycleManagement.Data;
using LifecycleManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Pages.ItemTypes;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context) => _context = context;

    [BindProperty]
    public ItemType ItemType { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        var itemType = await _context.ItemTypes.FindAsync(id);
        if (itemType == null) return NotFound();
        ItemType = itemType;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();
        var itemType = await _context.ItemTypes.FindAsync(id);
        if (itemType != null)
        {
            _context.ItemTypes.Remove(itemType);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage("./Index");
    }
}
