using System.Text;
using System.Text.Json;
using BookStore.Server.Data;
using BookStore.Server.Models;
using BookStore.Server.Services.AuthServices;
using BookStore.Server.Services.BookServices;
using BookStore.Server.Services.PaymentServices;
using BookStore.Server.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IBookServices, BookServices>();
builder.Services.AddTransient<IPaymentServices, PaymentServices>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Database
var _getConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_getConnectionString));
builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseNpgsql(_getConnectionString));

//Add identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    // If you're behind a load balancer, you can specify its IP address
    // options.KnownProxies.Add(IPAddress.Parse("loadBalancerIpAddress"));
});

//builder.Services.AddHttpsRedirection(options => { options.HttpsPort = 5126; });


//add authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
)
// add jwt bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
        ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]!))

    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 


app.Use(async (context, next) =>
{
    try
    {
        await next();
        if (context.Response.StatusCode == 401)
        {
            Console.WriteLine("Testing");
            context.Response.Clear();
            //context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 401;

            // Your sample data
            var data = new { message = "You are not authorized", status = "unsuccessful", };

            // Serialize the object to JSON
            var jsonData = JsonSerializer.Serialize(data);

            await context.Response.WriteAsJsonAsync(data);
        }
    }
    catch (Exception ex)
    {
    Console.WriteLine($"{ex} is the error");
    }
  
});

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();

