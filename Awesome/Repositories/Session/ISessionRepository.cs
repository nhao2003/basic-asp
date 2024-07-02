namespace Awesome.Repositories.Session;

public interface ISessionRepository : IRepository<Models.Entities.Session>
{
    Task<Models.Entities.Session?> GetByTokenAsync(string token);
}