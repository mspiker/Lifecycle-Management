using LifecycleManagement.Data;
using LifecycleManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Pages.Items;

[Authorize]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public Item Item { get; set; } = default!;

    public SelectList ItemTypes { get; set; } = default!;
    public SelectList Users { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
        if (item == null) return NotFound();

        Item = item;
        await LoadSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Item.ItemType");
        ModelState.Remove("Item.Owner");
        ModelState.Remove("Item.Notifications");

        if (!ModelState.IsValid)
        {
            await LoadSelectListsAsync();
            return Page();
        }

        _context.Attach(Item).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Items.AnyAsync(e => e.Id == Item.Id))
                return NotFound();
            throw;
        }

        return RedirectToPage("./Index");
    }

    private async Task LoadSelectListsAsync()
    {
        var types = await _context.ItemTypes.Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync();
        ItemTypes = new SelectList(types, "Id", "Name");

        var users = await _userManager.Users.OrderBy(u => u.DisplayName ?? u.Email).ToListAsync();
        Users = new SelectList(users.Select(u => new { u.Id, Name = u.DisplayName ?? u.Email }), "Id", "Name");
    }
}
