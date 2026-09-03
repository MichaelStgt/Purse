//using CommunityToolkit.Datasync.Server;
//using Microsoft.EntityFrameworkCore;
//using Purse.Server.Data;

//var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddDatasyncServices();
//builder.Services.AddControllers();

//var app = builder.Build();

//// Initialize the database
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    await context.InitializeDatabaseAsync().ConfigureAwait(false);
//}

//// Configure and run the web service.
//app.MapControllers();
//app.Run();


using Microsoft.EntityFrameworkCore;
using Purse.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// 2. Add DataSync Services
//builder.Services.AddDatasyncControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers(); // Maps the DataSync controllers automatically

app.Run();
