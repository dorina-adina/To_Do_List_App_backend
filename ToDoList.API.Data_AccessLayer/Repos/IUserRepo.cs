using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfo.API.DBLayer.Entities;

namespace ToDoListInfo.API.Data_AccessLayer.Repos
{
    public interface IUserRepo
    {
        Task<IEnumerable<Users>> GetUsersAsync();

        Task<Users> AddUser(Users user);

        Task<Users> GetUserAsync(string emailUser);
        Task<bool> UserExistsAsync(string emailUser);
        Task<bool> IsAdminAsync(string emailUser);

        Task<bool> SaveChangesAsync();

        //Task LoginGoogleAsync(ClaimsPrincipal? claimsPrincipal);


    }
}
