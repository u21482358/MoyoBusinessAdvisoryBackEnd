using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoyoBusinessAdvisory;
using MoyoBusinessAdvisory.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// https://stackoverflow.com/questions/56686093/unable-to-create-an-object-of-type-dbcontext#:~:text=Seems%20like%20you%20implemented%20IdentityContext%20but%20somewhere%20in%20your%20app%20its%20still%20trying%20to%20reference%20DbContext.%20Make%20sure%20Identitycontext%20is%20extending%20DbContext.
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    //options.User.re
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddDefaultPolicy(
    include =>
    {
        include.AllowAnyHeader();
        include.AllowAnyMethod();
        include.AllowAnyOrigin();
    }));


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// make sure builder stuff is before builder.Build()

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


