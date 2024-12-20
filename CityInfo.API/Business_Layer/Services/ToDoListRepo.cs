using Asp.Versioning.Conventions;
using CityInfo.API.Business_Layer.Models;
using CityInfo.API.Data_Access_Layer.Entities;
using CityInfo.API.DB_Layer.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Business_Layer.Services
{
    public class ToDoListRepo : IToDoListRepo
    {
        private readonly ToDoListContext _context;

        public ToDoListRepo(ToDoListContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ToDoList>> GetListsAsync()
        {
            var result = await _context.Database.SqlQueryRaw<ToDoList>("SELECT * FROM ToDoList").ToListAsync();

            return result;

        }

        public async Task<bool> ListExistsAsync(int listaId)
        {
            var result = await _context.Database.SqlQueryRaw<ToDoList>("SELECT * FROM ToDoList WHERE Id = listaId").AnyAsync();

            return result;
        }

        public async Task<ToDoList> GetListAsync(int listaId)
        {
            var result = await _context.Database.SqlQueryRaw<ToDoList>("SELECT * FROM ToDoList WHERE Id = " + listaId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void AddList(ToDoListForInsertDTO toDoList)
        {
            _context.Database.ExecuteSqlRaw("INSERT INTO ToDoList (Task, Priority, CreatedBy) VALUES (" + "'" + @toDoList.Task + "'" + "," + @toDoList.Priority +"," + "'" + @toDoList.Createdby + "'" + ")");
        }

        public void UpdateList(int id, ToDoListForUpdateDTO list)
        {
            _context.Database.ExecuteSqlRaw("UPDATE ToDoList SET Task =" + "'" + @list.Task + "'" + "," + "Priority =" + @list.Priority + "," + "Createdby =" + "'" + @list.Createdby + "'" + "WHERE Id =" + @id);

        }

        public void DeleteList(int id)
        {
            //var columnId = new SqlParameter("columnId", "https://7175");

            //_context.Database.SqlQueryRaw<ToDoList>($"DELETE FROM ToDoList WHERE Id = $id");

            //DECLARE @Id int

            //SET @Id = id;


            _context.Database.ExecuteSqlRaw("DELETE FROM ToDoList WHERE Id =" + @id);

            //_context.<ToDoList>.Where(l => l.Id == id).ExecuteDelete();
        }


    }
}
