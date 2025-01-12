using Asp.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;


namespace ToDoList.API.Data_AccessLayer.Services
{
    public class ToDoListRepo : IToDoListRepo
    {
        private readonly ToDoListContext _context;

        public ToDoListRepo(ToDoListContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Entities.ToDoList>> GetListsAsync()
        {
            var result = await _context.Database.SqlQueryRaw<Entities.ToDoList>("SELECT * FROM ToDoList").ToListAsync();

            return result;

        }

        public async Task<bool> ListExistsAsync(int listaId)
        {
            var result = await _context.Database.SqlQueryRaw<Entities.ToDoList>("SELECT * FROM ToDoList WHERE Id = listaId").AnyAsync();

            return result;
        }

        public async Task<Entities.ToDoList> GetListAsync(int listaId)
        {
            var result = await _context.Database.SqlQueryRaw<Entities.ToDoList>("SELECT * FROM ToDoList WHERE Id = " + listaId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void AddList(ToDoListForInsertDTO toDoList)
        {
            _context.Database.ExecuteSqlRaw("INSERT INTO ToDoList (Task, Priority, CreatedBy) VALUES (" + "'" + @toDoList.Task + "'" + "," + @toDoList.Priority + "," + "'" + @toDoList.CreatedBy + "'" + ")");
        }

        public void UpdateList(int id, ToDoListForUpdateDTO list)
        {
            _context.Database.ExecuteSqlRaw("UPDATE ToDoList SET Task =" + "'" + @list.Task + "'" + "," + "Priority =" + @list.Priority + "," + "Createdby =" + "'" + @list.Createdby + "'" + "WHERE Id =" + @id);

        }

        public void DeleteList(int id)
        {
            _context.Database.ExecuteSqlRaw("DELETE FROM ToDoList WHERE Id =" + @id);

        }


    }
}