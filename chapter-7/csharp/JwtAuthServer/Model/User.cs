namespace Model;

public record User(string Username, string HashedPassword, string Role)
{
    public bool IsCorrectPassword(string password)
    {
        var res = BetterPasswordHasher.VerifyHashedPassword(HashedPassword, password);
        if (res == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var newHash = BetterPasswordHasher.HashPassword(password);

            // store hash
        }
        return res != PasswordVerificationResult.Failed;
    }
}
