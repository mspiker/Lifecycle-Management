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
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public Item Item { get; set; } = new();

    public SelectList ItemTypes { get; set; } = default!;
    public SelectList Users { get; set; } = default!;

    public async Task OnGetAsync()
    {
        await LoadSelectListsAsync();
        Item.ExpirationDate = DateTime.Today.AddYears(1);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Item.ItemType");
        ModelState.Remove("Item.Owner");
        ModelState.Remove("Item.Notifications");
        ModelState.Remove("Item.OwnerUserId");

        if (!ModelState.IsValid)
        {
            await LoadSelectListsAsync();
            return Page();
        }

        Item.CreatedDate = DateTime.UtcNow;
        _context.Items.Add(Item);
        await _context.SaveChangesAsync();
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
