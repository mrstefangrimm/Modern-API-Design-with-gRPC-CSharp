using Microsoft.IdentityModel.Tokens;
using Repo;
using System;
using System.Text;

namespace Config;

public static class Configs
{
    public static readonly SymmetricSecurityKey SecurityKey = new(Encoding.UTF8.GetBytes("long-secret-is-required-256-minimum"));
    public static readonly TimeSpan TokenDuration = TimeSpan.FromMinutes(15);

    public static void SeedUsers(UserStore userStore)
    {
        var user = Service.Service.NewUser("admin1", "12345", "admin");

        userStore.Save(user);
    }
}
