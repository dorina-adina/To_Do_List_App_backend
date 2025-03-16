using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.Data_AccessLayer.Entities;

namespace ToDoListInfo.API.DBLayer.DbContexts
{
    public class UserInfoContext : DbContext
    {
        public DbSet<Users> Users { get; set; }

        public UserInfoContext(DbContextOptions<UserInfoContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-3MH72LO\\SQLEXPRESS;Database=adina_todos;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer("Server=DESKTOP-0FC0IG4\\SQLEXPRESS01;Database=DBTest;User Id=sa;Password=BT.Cj#9628517;MultipleActiveResultSets=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
