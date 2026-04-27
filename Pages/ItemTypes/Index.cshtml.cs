using LifecycleManagement.Data;
using LifecycleManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Pages.ItemTypes;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context) => _context = context;

    public List<ItemType> ItemTypes { get; set; } = new();

    public async Task OnGetAsync()
    {
        ItemTypes = await _context.ItemTypes.OrderBy(t => t.Name).ToListAsync();
    }
}
