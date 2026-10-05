using blog.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
var conectionString = builder.Configuration.GetConnectionString("BaseConnection");
builder.Services.AddDbContext<MyDbContext>(options =>options.UseNpgsql(conectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.Run();

