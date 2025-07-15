using ToDoListInfo.API.DBLayer.Entities;

namespace ToDoListInfo.API.Data_AccessLayer.Repos
{
    public interface IToDoListRepo
    {
        Task<IEnumerable<ToDoList>> GetLists();

        Task<bool> ListExistsAsync(int listaId);

        Task<ToDoList> GetListAsync(int listaId);

        Task<IEnumerable<ToDoList>> GetListCreatedByAsync(int idOwner);

        Task<ToDoList> AddList(ToDoList toDoList);

        Task<ToDoList> UpdateList(int id, ToDoList toDoList);

        void DeleteList(int id);

        Task<bool> SaveChangesAsync();

        Task<Upload> AddFileAsync(string fileName, string filePath, int idOwner, string emailOwner, string infoPath, int idTask);

        Task<IEnumerable<Upload>> GetFiles();

        Task<IEnumerable<Upload>> GetFilesCreatedByAsync(int idOwner);
        Task<Upload> GetFileById(int id);



    }
}