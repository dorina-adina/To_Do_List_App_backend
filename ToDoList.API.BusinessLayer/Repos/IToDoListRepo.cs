using ToDoListInfo.API.BusinessLayer.Models;

namespace ToDoListInfo.API.BusinessLayer.Repos
{
    public interface IToDoListRepo
    {
        Task<IEnumerable<ToDoListDTO>> GetListsAsync();

        Task<bool> ListExistsAsync(int listaId);

        Task<ToDoListDTO> GetListAsync(int listaId);
        void AddList(ToDoListForInsertDTO toDoList);

        void UpdateList(int id, ToDoListForUpdateDTO toDoList);

        void DeleteList(int id);

        Task<bool> SaveChangesAsync();

        void AddFile(UploadDTO file);




    }
}