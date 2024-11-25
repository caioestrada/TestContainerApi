using Carter;
using System.Data;
using System.Data.SqlClient;
using TestContainerApi.Domain.Users.Get;
using TestContainerApi.Domain.Users.Get.ApiServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddScoped<IGetUsersRepository, GetUsersRepository>();
builder.Services.AddScoped<IGetUsersService, GetUsersService>();
builder.Services.AddTransient<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

builder.Services.AddHttpClient<ICatsApiService, CatsApiService>(sp =>
{
    sp.BaseAddress = new Uri("https://api.thecatapi.com/v1/");
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapCarter();

app.Run();

public partial class Program { }