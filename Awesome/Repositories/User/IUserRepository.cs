namespace Awesome.Repositories.User;

public interface IUserRepository : IRepository<Models.Entities.User>
{
    Task<Models.Entities.User?> GetByUsername(string username);
}