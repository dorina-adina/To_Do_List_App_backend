using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.DBLayer.Entities;

namespace ToDoListInfo.API.DBLayer.DbContexts
{
    
    public class ToDoListInfoContext : DbContext
    {
        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<Priorities> Priorities { get; set; }
        public DbSet<Upload> Upload { get; set; }
        public DbSet<Users> Users { get; set; }

        public ToDoListInfoContext(DbContextOptions<ToDoListInfoContext> options)
            : base(options)
        {
        }
    }

}

