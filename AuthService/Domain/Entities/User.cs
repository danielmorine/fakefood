using System;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    private User() { }

    public User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = password;
    }    
}
