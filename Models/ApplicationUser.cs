using Microsoft.AspNetCore.Identity;

namespace LifecycleManagement.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string ThemePreference { get; set; } = "System";
    public ICollection<Item> OwnedItems { get; set; } = new List<Item>();
}
