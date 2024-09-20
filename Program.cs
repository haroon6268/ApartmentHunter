using System.Text;
using System.Text.Json;
using Apartments.Middleware;
using Apartments.Models;
using Apartments.Services;
using ApartmentsApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.IO;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApartmentsDatabaseSettings>(builder.Configuration.GetSection("ApartmentsDatabase"));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents{
        OnChallenge = context => {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new {error="unauthorized"});
            return context.Response.WriteAsync(result);
        }
    };
});
builder.Services.AddSingleton<ListingService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<PasswordService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddControllers();
builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",
        builder =>{
            builder.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
        }
    );
});
var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseMiddleware<JwtTokenCookieMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserClaimsToHeader>();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

