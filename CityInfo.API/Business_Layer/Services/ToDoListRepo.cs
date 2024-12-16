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
            var result = await _context.Database.SqlQueryRaw<ToDoList>("SELECT lista FROM ToDoList WHERE lista.id = listaId").AnyAsync();

            return result;
        }

        public async Task<ToDoList> GetListAsync(int listaId)
        {
            var result = await _context.Database.SqlQueryRaw<ToDoList>("SELECT lista FROM ToDoList WHERE lista.id = listaId").FirstOrDefaultAsync();

            return result;
        }

        public void DeleteList(ToDoList listaa)
        {
            _context.Database.SqlQueryRaw<ToDoList>("DELETE lista FROM ToDoList WHERE lista.id == listaa.id");
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

    }
}
