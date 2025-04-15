using Model;

namespace Service;

public static class Service
{
    public static User NewUser(string username, string password, string role)
    {
        var hashedPassword = BetterPasswordHasher.HashPassword(password);
        
        return new User(username, hashedPassword, role);
    }
}
