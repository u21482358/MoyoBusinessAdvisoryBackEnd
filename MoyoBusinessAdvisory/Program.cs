using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoyoBusinessAdvisory;
using MoyoBusinessAdvisory.Models;
using Newtonsoft.Json.Serialization;
using System.Security.Policy;
using System.Text;
//using Microsoft.AspNetCore.Mvc.New

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddCors(options => options.AddDefaultPolicy(
//    include =>
//    {
//        include.AllowAnyHeader();
//        include.AllowAnyMethod();
//        include.AllowAnyOrigin();
//    }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// https://stackoverflow.com/questions/56686093/unable-to-create-an-object-of-type-dbcontext#:~:text=Seems%20like%20you%20implemented%20IdentityContext%20but%20somewhere%20in%20your%20app%20its%20still%20trying%20to%20reference%20DbContext.%20Make%20sure%20Identitycontext%20is%20extending%20DbContext.
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = false;
    //options.User.re
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();
//https://stackoverflow.com/questions/59199593/net-core-3-0-possible-object-cycle-was-detected-which-is-not-supported
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// added in https://stackoverflow.com/questions/67974556/system-text-json-jsonexception-a-possible-object-cycle-was-detected
builder.Services.AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.ReferenceHandler = null;
                });


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = builder.Configuration["Tokens:Issuer"],
                        ValidAudience = builder.Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
                    };
                }).AddGoogle(options =>
                {

          
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]; ;
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]; ;
                }); ;

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// make sure builder stuff is before builder.Build()

var app = builder.Build();
app.UseCors("AllowAllHeaders");


//});
app.UseAuthentication();

app.UseAuthorization();
//app.UseRouting();
//https://stackoverflow.com/questions/57164127/the-oauth-state-was-missing-or-invalid-an-error-was-encountered-while-handling


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


