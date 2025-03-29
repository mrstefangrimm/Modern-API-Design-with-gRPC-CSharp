using Model;

namespace Repo;

public interface IUserStore
{
    User Find(string username);
    void Save(User user);
}
