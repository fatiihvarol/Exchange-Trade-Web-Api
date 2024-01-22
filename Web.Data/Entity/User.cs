using Base.Entity;

namespace Web.Data.Entity;

public class User:BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }

    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public decimal Budget { get; set; }
    
    public Portfolio Portfolio { get; set; }
}