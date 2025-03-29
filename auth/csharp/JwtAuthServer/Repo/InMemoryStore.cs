using Model;
using System;
using System.Collections.Generic;

namespace Repo;

public class InMemoryStore
{
    private readonly Dictionary<string, User> _userStore = new();
    private readonly object _lockObject = new();

    public bool Save(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(user.Username);

        lock (_lockObject)
        {
            if (_userStore.ContainsKey(user.Username))
            {
                return false;
            }

            _userStore[user.Username] = user;
        }
        return true;
    }

    public User Find(string username)
    {
        lock (_lockObject)
        {
            return _userStore[username];
        }
    }
}
