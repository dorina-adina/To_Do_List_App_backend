using CityInfo.API.Data_Access_Layer.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DB_Layer.DbContexts
{
    public class ToDoListContext : DbContext
    {
        public DbSet<ToDoList> ToDoLists { get; set; }

        public ToDoListContext(DbContextOptions<ToDoListContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=BTCCLPF1PMR0J\\SQLTESTSERVER;Database=DbTest;User Id=sa;Password=BT.Cj#9628517;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Server=DESKTOP-0FC0IG4\\SQLEXPRESS01;Database=DBTest;User Id=sa;Password=BT.Cj#9628517;TrustServerCertificate=True;");

            base.OnConfiguring(optionsBuilder);
        }
    }

}

