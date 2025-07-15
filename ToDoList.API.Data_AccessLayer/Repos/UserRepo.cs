using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.DBLayer.DbContexts;
using ToDoListInfo.API.DBLayer.Entities;
using System.Security.Claims;

namespace ToDoListInfo.API.Data_AccessLayer.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly ToDoListInfoContext _context;

        // initializare, DI
        public UserRepo(ToDoListInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // se obtin toti userii
        public async Task<IEnumerable<Users>> GetUsersAsync()
        {

            return _context.Set<Users>();

        }

        // salveaza modificarile facute in EF la baza de date
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        // se adauga un user
        public async Task<Users> AddUser(Users user)
        {

            string newPass = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Pass, 13);

            user.Pass = newPass;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;

        }

        // se obtine un user dupa email
        public async Task<Users> GetUserAsync(string emailUser)
        {
            
            var user = await _context.Users.Where(l => l.Email == emailUser).FirstOrDefaultAsync();
            return user;

        }

        // se verifica existenta unui user dupa email
        public async Task<bool> UserExistsAsync(string emailUser)
        {
            var result = await _context.Users.Where(l => l.Email == emailUser).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }
            return true;
        }

        // se verifica daca user-ul e admin
        public async Task<bool> IsAdminAsync(string emailUser)
        {
            var result = await _context.Users.Where(l => l.Email == emailUser).FirstOrDefaultAsync();

            if (result == null || result.IsAdmin == false)
            {
                return false;
            }
            return true;
        }

        // modificare parola
        public async Task<Users> ChangePasswordUser(string newPass, string email)
        {
            var userToUpdate = await _context.Users.Where(l => l.Email == email).FirstOrDefaultAsync();

            string newPassHashed = BCrypt.Net.BCrypt.EnhancedHashPassword(newPass, 13);

            if (userToUpdate != null)
            {
                userToUpdate.Pass = newPassHashed;
            }

            await _context.SaveChangesAsync();

            return userToUpdate;
        }

        // login Google
        public async Task<bool> LoginGoogleAsync(ClaimsPrincipal? claimsPrincipal)
        {
               if (claimsPrincipal == null)
                {
                    throw new Exception();
                }

                var email = claimsPrincipal.FindFirst(ClaimTypes.Email);

                if (email == null)
                {
                    throw new Exception();
                }
                var user = await _context.Users.FindAsync(email);

                if (user == null)
                {
                    return false;
                }
                return true;
        }
    }
}

