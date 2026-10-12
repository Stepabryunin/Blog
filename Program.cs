using Blog;
using blog.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Blog.Services;
using Blog.Mapper;
using Blog.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var jWTparametres= new JWTOptions(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
        options.TokenValidationParameters=jWTparametres.GetOptions();
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IRefreshTokenService,RefreshTokenService>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<PostMapper>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<LikeMapper>();
builder.Services.AddScoped<ILikeInterface, LikeService>();
builder.Services.AddScoped<IFollowerService, FollowerService>();
builder.Services.AddControllers();

builder.Services.AddOpenApi();
var conectionString = builder.Configuration.GetConnectionString("BaseConnection");
builder.Services.AddDbContext<MyDbContext>(options =>options.UseNpgsql(conectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", "Blog API v1"));
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

