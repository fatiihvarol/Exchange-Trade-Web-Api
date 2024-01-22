namespace Web.Schema;

public class UserRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }

    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public decimal Budget { get; set; }
}

public class UserResponse
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }

    public string UserName { get; set; }
    public string Role { get; set; }
    public decimal Budget { get; set; }
}