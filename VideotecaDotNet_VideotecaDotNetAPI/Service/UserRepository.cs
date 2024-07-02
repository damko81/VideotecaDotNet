
using VideotecaDotNet_VideotecaDotNetAPI.Data;
using VideotecaDotNet_VideotecaDotNetAPI.Models;

namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private List<Users> _users = new List<Users>() { new Users {UserName="admin", Password="password"} };

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
            List<Users> users = _db.Users.ToList();
            foreach (Users user in users)
            {
                Users u = new Users();
                u.Id = user.Id;
                u.Name = user.Name;
                u.UserName = user.UserName;
                u.Password = BusinessService.DecodeFrom64(user.Password);

                _users.Add(u);
            }
        }

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
