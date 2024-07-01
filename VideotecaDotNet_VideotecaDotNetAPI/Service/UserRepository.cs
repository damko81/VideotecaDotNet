
using VideotecaDotNet_VideotecaDotNetAPI.Models;

namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public class UserRepository : IUserRepository
    {
        private List<Users> _users = new List<Users>
        {
            new Users
            {
                Id = 6, Name="Damjan Koscak", UserName = "damko81", Password = "biceps"
            },
            
            new Users
            {
                Id = 5, Name="Martina Koscak", UserName = "martinka", Password = "martinka"
            }
        };
        public async Task<bool> Authenticate(string username, string password)
        {
            if (await Task.FromResult(_users.SingleOrDefault(x => x.UserName == username && x.Password == password)) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<string>> GetUserNames()
        {
            List<string> users = new List<string>();
            foreach (var user in _users)
            {
                users.Add(user.UserName);
            }
            return await Task.FromResult(users);
        }
    }
}
