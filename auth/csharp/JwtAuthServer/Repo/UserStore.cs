using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repo;

public class UserStore : IUserStore
{
    private readonly List<User> _users = new();

    public User Find(string username)
    {
        return _users.FirstOrDefault(x => x.Username == username);
    }

    public void Save(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        _users.Add(user);
    }
}
