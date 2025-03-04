using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Entities;

namespace ToDoListInfo.API.BusinessLayer.Repos
{
    public interface IToDoListRepo
    {
        Task<IEnumerable<ToDoListDTO>> GetListsAsync();

        Task<bool> ListExistsAsync(int listaId);

        Task<ToDoListDTO> GetListAsync(int listaId);

        Task<IEnumerable<ToDoListDTO>> GetListCreatedByAsync(int idOwner);

        void AddList(ToDoListForInsertDTO toDoList);

        void UpdateList(int id, ToDoListForUpdateDTO toDoList);

        void DeleteList(int id);

        Task<bool> SaveChangesAsync();

        Task<Upload> AddFileAsync(string fileName, string filePath);





    }
}