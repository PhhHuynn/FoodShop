using ASM.Server.Data;
using ASM.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ASM.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// COR
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins("http://localhost:59257") 
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

builder.Services.AddSignalR();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(connectionString)
);

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT
var jwtSetting = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSetting["Key"]);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSetting["Issuer"],
		ValidAudience = jwtSetting["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(key)
	};
})
.AddGoogle(googleOptions =>
 {
	 googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
	 googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
 });

// CORS
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.WithOrigins("https://localhost:59257")
			  .AllowAnyHeader()
			  .AllowAnyMethod()
			  .AllowCredentials();
	});
});


// Policy
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
	options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

var app = builder.Build();

app.MapHub<ChatHub>("/chathub");

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	string[] roleNames = { "Admin", "User" };

	foreach (var roleName in roleNames)
	{
		var roleExist = await roleManager.RoleExistsAsync(roleName);
		if (!roleExist)
		{
			await roleManager.CreateAsync(new IdentityRole(roleName));
		}
	}
}

app.UseStaticFiles();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapFallbackToFile("/index.html");

app.MapControllers();

app.Run();
