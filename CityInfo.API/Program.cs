using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using ToDoList.API;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using ToDoListInfo.API.Data_AccessLayer.Services;
using ToDoListInfo.API.DBLayer.DbContexts;
using ToDoListInfo.API.BusinessLayer.Repos;
using System.Data.Common;
using Microsoft.Extensions.Configuration.Json;
using ToDoListInfo.API.DBLayer;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateFormatString = "dd-MM-yyyy"; 
})
.AddXmlDataContractSerializerFormatters();

builder.Services.AddProblemDetails();

// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailServices>();
#endif

builder.Services.AddDbContext<ToDoListInfoContext>(options =>
    options.UseSqlServer(
//builder.Configuration.GetConnectionString("Server=BTCCLPF1PMR0J\\SQLTESTSERVER;Database=DbTest;User Id=sa;Password=BT.Cj#9628517;TrustServerCertificate=True;")));
//builder.Configuration.GetConnectionString("Server=DESKTOP-0FC0IG4\\SQLEXPRESS01;Database=DBTest;User Id=sa;Password=adina;MultipleActiveResultSets=True;TrustServerCertificate=True;")));
builder.Configuration.GetConnectionString("dbConnection")));

builder.Services.AddDbLayerDependencies(
    builder.Configuration);

builder.Services.AddScoped<IToDoListRepo, ToDoListRepo>();





//builder.Services.AddDbContext<UserInfoContext>(options =>
//    options.UseSqlServer(
////builder.Configuration.GetConnectionString("Server=BTCCLPF1PMR0J\\SQLTESTSERVER;Database=DbTest;User Id=sa;Password=BT.Cj#9628517;TrustServerCertificate=True;")));
//builder.Configuration.GetConnectionString("Server=DESKTOP-0FC0IG4\\SQLEXPRESS01;Database=DBTest;User Id=sa;Password=adina;MultipleActiveResultSets=True;TrustServerCertificate=True;")));

builder.Services.AddScoped<IUserRepo, UserRepo>();




builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
               Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))
    };

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeFromAntwerp", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Antwerp");
    });
});

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
}).AddMvc()
.AddApiExplorer(setupAction =>
{
    setupAction.SubstituteApiVersionInUrl = true;
});


var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
  .GetRequiredService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction =>
{
    foreach (var description in
        apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc(
            $"{description.GroupName}",
            new()
            {
                Title = "To-Do List API",
                Version = description.ApiVersion.ToString(),
                Description = "Through this API you can access yours to-do lists."
            });
    }

    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);

    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });

    setupAction.AddSecurityRequirement(new()
    {
        {
            new ()
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth" }
            },
            new List<string>()
        }
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
    | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            setupAction.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();