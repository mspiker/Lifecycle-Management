using LifecycleManagement.Data;
using LifecycleManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LifecycleManagement.Pages.ItemTypes;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context) => _context = context;

    [BindProperty]
    public ItemType ItemType { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("ItemType.Items");
        if (!ModelState.IsValid) return Page();

        _context.ItemTypes.Add(ItemType);
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
