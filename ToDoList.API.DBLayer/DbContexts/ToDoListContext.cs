using Microsoft.EntityFrameworkCore;

namespace ToDoList.API.DBLayer.DbContexts
{
    
    public class ToDoListContext : DbContext
    {
        public DbSet<Data_AccessLayer.Entities.ToDoList> ToDoLists { get; set; }

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

