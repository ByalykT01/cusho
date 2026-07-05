namespace cusho.Models;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string KeycloakId { get; init; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public DateTime CreatedAt { get; init; }

    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<WishlistItem> WishlistItems { get; set; } = [];
}
