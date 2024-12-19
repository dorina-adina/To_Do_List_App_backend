using CityInfo.API.Business_Layer.Models;
using CityInfo.API.Data_Access_Layer.Entities;

namespace CityInfo.API.Business_Layer.Services
{
    public interface IToDoListRepo
    {
        Task<IEnumerable<ToDoList>> GetListsAsync();

        Task<bool> ListExistsAsync(int listaId);

        Task<ToDoList> GetListAsync(int listaId);

        //void UpdateList(ToDoListForUpdateDTO toDoList);

        void DeleteList(int id);

        Task<bool> SaveChangesAsync();




    }
}
