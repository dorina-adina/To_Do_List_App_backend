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
            //optionsBuilder.UseSqlServer("Server=BTCCLPF1PMR0J\\SQLTESTSERVER;Database=DbTest;User Id=sa;Password=BT.Cj#9628517;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Server=DESKTOP-0FC0IG4\\SQLEXPRESS01;Database=DBTest;User Id=sa;Password=BT.Cj#9628517;MultipleActiveResultSets=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
