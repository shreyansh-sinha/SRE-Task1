using StudentAPI.Extension;
using StudentAPI.Model;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// method to configure all services (in our case database)
void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(builder.Configuration);

    DbConfiguration dbConfiguration = new();
    builder.Configuration.GetSection("CosmosDbSettings").Bind(dbConfiguration);

    services.RegisterDbDependencies(dbConfiguration).GetAwaiter().GetResult();
}