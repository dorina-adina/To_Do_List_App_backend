using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.Data_AccessLayer.Entities;

namespace ToDoListInfo.API.DBLayer.DbContexts
{
    
    public class ToDoListInfoContext : DbContext
    {
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<Priorities> Priorities { get; set; }
        public DbSet<Upload> Upload { get; set; }
        public DbSet<Users> Users { get; set; }

        public ToDoListInfoContext(DbContextOptions<ToDoListInfoContext> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("Server=BTCCLPF1PMR0J\\SQLTESTSERVER;Database=DbTest;User Id=sa;Password=BT.Cj#9628517;TrustServerCertificate=True;");
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-0FC0IG4\\SQLEXPRESS01;Database=DBTest;User Id=sa;Password=adina;MultipleActiveResultSets=True;TrustServerCertificate=True;");
        //    //optionsBuilder.UseSqlServer("dbConnection");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }

}

