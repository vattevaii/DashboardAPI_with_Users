using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using DashboardAPI.Data;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DashboardAPIContext>(options => 
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DashboardAPIContext") ??
            throw new InvalidOperationException("Connection string 'DashboardAPIContext' not found."));
    });

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value
                )),
            ValidateAudience = false,
            ValidateIssuer = false
        };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("User", p => p.RequireRole("User","Admin"));
    o.AddPolicy("Admin", p => p.RequireRole("Admin"));
});

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline. Middlewares for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares for app execution
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


// Only use for public static files
//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(builder.Environment.ContentRootPath, "Static")),
//    RequestPath = "/Static"
//});


// Map Controllers 
app.MapControllers();


// Run the app
app.Run();
