using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ClientPortalApi.Data;
using ClientPortalApi.Services;
using ClientPortalApi.Hubs;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using ClientPortalApi.Models;
using Microsoft.AspNetCore.StaticFiles;
using ClientPortalApi.Services.Notifications;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddScoped<IProjectInvitationService, ProjectInvitationService>();

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
	{
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer"
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
		   {
			 {
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
				},
				[]
			 }
		   });
});


// DbContext - SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection") ?? "Data Source=clientportal.db"));

// CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowReactDev", p =>
//    {
//        p.WithOrigins(configuration["ReactClientUrl"] ?? "http://localhost:3000")
//         .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
//    });
//});

// Add CORS for SignalR
builder.Services.AddCors(options =>
{
	options.AddPolicy("SignalRCors", policy =>
	{
        policy.WithOrigins(
                "http://localhost:8081",
				"http://localhost:8082",
				"https://preview--integrated-suite.lovable.app",
				"https://preview-81218cb1--integrated-suite.lovable.app"
			)
            .AllowAnyHeader()
            .AllowAnyMethod()
			.AllowCredentials();
	});
});

// JWT
var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "ReplaceWithLongSecretKeyForProd_ChangeThis");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = configuration["Jwt:Issuer"] ?? "ClientPortalApi",
        ValidAudience = configuration["Jwt:Audience"] ?? "ClientPortalClient",
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
	// Configure SignalR to use JWT from query string for WebSockets
	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = context =>
		{
			var accessToken = context.Request.Query["access_token"];
			var path = context.HttpContext.Request.Path;

			if (!string.IsNullOrEmpty(accessToken) &&
				path.StartsWithSegments("/hubs/notifications"))
			{
				context.Token = accessToken;
			}
			return Task.CompletedTask;
		}
	};
});

// DI
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<INotificationHubService, NotificationHubService>();

var app = builder.Build();

// Ensure DB created and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbSeeder.SeedAsync(db, new PasswordHasher<User>()).GetAwaiter().GetResult();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("SignalRCors");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/hubs/notifications").RequireAuthorization();

app.MapControllers();


app.Run();
