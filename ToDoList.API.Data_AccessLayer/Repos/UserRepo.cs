using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.DBLayer.DbContexts;
using System.Security.Cryptography;
using ToDoListInfo.API.DBLayer.Entities;
using Azure.Identity;
using System.Security.Claims;

namespace ToDoListInfo.API.Data_AccessLayer.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly ToDoListInfoContext _context;
        //private readonly byte[] encryptionKey;
        //private readonly byte[] iv;

        public UserRepo(ToDoListInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Users>> GetUsersAsync()
        {

            return _context.Set<Users>();

        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }


        public async Task<Users> AddUser(Users user)
        {

            string newPass = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Pass, 13);

            //byte[] cyphertextBytes;
            //using Aes aes = Aes.Create();
            //aes.Key = encryptionKey;
            //aes.IV = iv;
            //var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            //using (var memoryStream = new MemoryStream())
            //{
            //    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            //    {
            //        using (var streamWriter = new StreamWriter(cryptoStream))
            //        {
            //            streamWriter.Write(user.Pass);
            //        }
            //    }
            //    cyphertextBytes = memoryStream.ToArray();

            //    var newPass = Convert.ToBase64String(cyphertextBytes);


            user.Pass = newPass;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            //}

            return user;



        }




        public async Task<Users> GetUserAsync(string emailUser)
        {
            //var result = _context.Set<Users>();

            //List<Users> users = new List<Users>();

            //foreach (Users item in result)
            //{
            //    if (item.Email == emailUser)
            //        users.(item);
            //}

            //return users;

            var user = await _context.Users.Where(l => l.Email == emailUser).FirstOrDefaultAsync();
            return user;

        }

        public async Task<bool> UserExistsAsync(string emailUser)
        {
            var result = await _context.Users.Where(l => l.Email == emailUser).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsAdminAsync(string emailUser)
        {
            var result = await _context.Users.Where(l => l.Email == emailUser).FirstOrDefaultAsync();

            if (result == null || result.IsAdmin == false)
            {
                return false;
            }
            return true;
        }

    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using ToDoListInfo.API.DBLayer.DbContexts;
//using System.Security.Cryptography;
//using ToDoListInfo.API.DBLayer.Entities;
//using System.Security.Claims;

//namespace ToDoListInfo.API.Data_AccessLayer.Repos
//{
//    public class UserRepo : IUserRepo
//    {
//        private readonly ToDoListInfoContext _context;

//        public UserRepo(ToDoListInfoContext context)
//        {
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//        }

//        public async Task<IEnumerable<Users>> GetUsersAsync()
//        {

//            return _context.Set<Users>();

//        }

//        public async Task<bool> SaveChangesAsync()
//        {
//            return await _context.SaveChangesAsync() >= 0;
//        }

//        public async Task<Users> AddUser(Users user)
//        {

//            string newPass = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Pass, 13);
//            user.Pass = newPass;

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();
//            return user;

//        }

//        public async Task<Users> GetUserAsync(string emailUser)
//        {
//            var result = _context.Set<Users>();

//            List<Users> users = new List<Users>();

//            foreach (Users item in result)
//            {
//                if (item.Email == emailUser)
//                    users.(item);
//            }

//            return users;
//        }

//        public async Task<bool> UserExistsAsync(string emailUser)
//        {
//            var result = await _context.Users.FindAsync(emailUser);

//            if (result == null)
//            {
//                return false;
//            }
//            return true;
//        }

//        public async Task<bool> IsAdminAsync(string emailUser)
//        {
//            var result = await _context.Users.FindAsync(emailUser);

//            if (result == null || result.IsAdmin == false)
//            {
//                return false;
//            }
//            return true;
//}

//    public async Task LoginGoogleAsync(ClaimsPrincipal? claimsPrincipal)
//    {
//        if (claimsPrincipal == null)
//        {
//            throw new Exception();
//        }

//        var email = claimsPrincipal.FindFirst(ClaimTypes.Email);

//        if (email == null)
//        {
//            throw new Exception();
//        }
//        var user = await _context.Users.FindAsync(email);

//        if (user == null)
//        {
//            var newUser = new Users
//            {
//                Email = email,

//            };
//        }
//    }
//}
//}

