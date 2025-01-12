
namespace ToDoList.API.Data_AccessLayer.Services
{
    public interface IToDoListRepo
    {
        Task<IEnumerable<Entities.ToDoList>> GetListsAsync();

        Task<bool> ListExistsAsync(int listaId);

        Task<Entities.ToDoList> GetListAsync(int listaId);

        void AddList(ToDoListForInsertDTO toDoList);

        void UpdateList(int id, ToDoListForUpdateDTO toDoList);

        void DeleteList(int id);

        Task<bool> SaveChangesAsync();




    }
}