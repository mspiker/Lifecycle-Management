using LifecycleManagement.Data;
using LifecycleManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Pages.ItemTypes;

[Authorize]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context) => _context = context;

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

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("ItemType.Items");
        if (!ModelState.IsValid) return Page();

        _context.Attach(ItemType).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.ItemTypes.AnyAsync(e => e.Id == ItemType.Id))
                return NotFound();
            throw;
        }

        return RedirectToPage("./Index");
    }
}
