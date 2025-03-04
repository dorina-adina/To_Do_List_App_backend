using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Entities;
using ToDoListInfo.API.DBLayer.DbContexts;

namespace ToDoListInfo.API.BusinessLayer.Repos
{
    public class ToDoListRepo : IToDoListRepo
    {
        private readonly ToDoListInfoContext _context;

        public ToDoListRepo(ToDoListInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ToDoListDTO>> GetListsAsync()
        {
            var result = await _context.Database.SqlQueryRaw<ToDoListDTO>("SELECT * FROM ToDoList").ToListAsync();

            return result;

        }

        public async Task<bool> ListExistsAsync(int listaId)
        {
            var result = await _context.Database.SqlQueryRaw<ToDoListDTO>("SELECT * FROM ToDoList WHERE Id = listaId").AnyAsync();

            return result;
        }

        public async Task<ToDoListDTO> GetListAsync(int listaId)
        {
            var result = await _context.Database.SqlQueryRaw<ToDoListDTO>("SELECT * FROM ToDoList WHERE Id = " + listaId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<ToDoListDTO>> GetListCreatedByAsync(string owner)
        {
            var result = await _context.Database.SqlQueryRaw<ToDoListDTO>("SELECT * FROM ToDoList WHERE CreatedBy = " + "'" + owner + "'").ToListAsync();

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
 

        public async Task<Upload> AddFileAsync(string fileName, string filePath)
        {
            var fileUpload = new Upload
            {
                Name = fileName,
                Path = filePath,
                CreatedDate = DateTime.Now
            };

            _context.Upload.Add(fileUpload);
            await _context.SaveChangesAsync();

            return fileUpload;
        }
    }
}