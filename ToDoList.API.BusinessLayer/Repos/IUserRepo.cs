using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Entities;

namespace ToDoListInfo.API.BusinessLayer.Repos
{
    public interface IUserRepo
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();

        void AddUser(UserInsertDTO user);

        //void UpdateList(int id, ToDoListForUpdateDTO toDoList);


        Task<UserDTO> GetUserAsync(string emailUser);
        Task<bool> UserExistsAsync(string emailUser);
        Task<bool> IsAdminAsync(string emailUser);

        Task<bool> SaveChangesAsync();


    }
}
