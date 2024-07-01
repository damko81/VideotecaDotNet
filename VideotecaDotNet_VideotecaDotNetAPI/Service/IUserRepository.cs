
namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public interface IUserRepository
    {
        Task<bool> Authenticate(string username, string password);
        Task<List<string>> GetUserNames();
    }
}
