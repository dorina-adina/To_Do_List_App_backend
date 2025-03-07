using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Entities;
using ToDoListInfo.API.DBLayer.DbContexts;
using System.Security.Cryptography;

namespace ToDoListInfo.API.BusinessLayer.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly UserInfoContext _context;

        public UserRepo(UserInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var result = await _context.Database.SqlQueryRaw<UserDTO>("SELECT * FROM Users").ToListAsync();

            return result;

        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void AddUser(UserInsertDTO user)
        {
            //    SHA256 hash = SHA256.Create();

            //    var passBytes = Encoding.Default.GetBytes(user.Pass);

            //    var hashedPass = hash.ComputeHash(passBytes);

            //    var newPass = Convert.ToHexString(hashedPass);

            string newPass = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Pass, 13);

            _context.Database.ExecuteSqlRaw("INSERT INTO Users (Name, PhoneNr, IsAdmin, Email, Pass) VALUES (" + "'" + @user.Name + "'" + "," + "'" + @user.PhoneNr + "'" + "," + "'" + @user.IsAdmin + "'" + "," + "'" + @user.Email + "'" + "," + "'" + newPass + "'" + ")");
        }

        public async Task<UserDTO> GetUserAsync(string emailUser)
        {
            var result = await _context.Database.SqlQueryRaw<UserDTO>("SELECT * FROM Users WHERE Email = " + "'" + emailUser + "'").FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> UserExistsAsync(string emailUser)
        {
            var result = await _context.Database.SqlQueryRaw<UserDTO>("SELECT * FROM Users WHERE Email = " + "'" + emailUser + "'").AnyAsync();

            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsAdminAsync(string emailUser)
        {
            var result = await _context.Database.SqlQueryRaw<UserDTO>("SELECT * FROM Users WHERE Email = " + "'" + emailUser + "'").FirstOrDefaultAsync();

            if (result == null || result.IsAdmin == false)
            {
                return false;
            }
            return true;
        }

    }
}
