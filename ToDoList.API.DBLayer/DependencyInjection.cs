using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoListInfo.API.DBLayer.DbContexts;

namespace ToDoListInfo.API.DBLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbLayerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStr = configuration.GetConnectionString("dbConnection");
            services.AddDbContext<ToDoListInfoContext>(options =>
    options.UseSqlServer(connectionStr));
            return services;


        }
    }
}
