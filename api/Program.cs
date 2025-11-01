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
using Stripe;
using TokenService = ClientPortalApi.Services.TokenService;
using FileService = ClientPortalApi.Services.FileService;
using Stripe.Forwarding;
using Newtonsoft.Json;
using ClientPortalApi.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static ClientPortalApi.Utils.Money;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddSingleton<IStripeService, StripeService>();

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
				Environment.GetEnvironmentVariable("ASPNETCORE_REACTAPPURL")!
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
builder.Services.AddScoped<INotificationHubService, NotificationHubService>();

var app = builder.Build();

// Ensure DB created and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbSeeder.SeedAsync(db, new PasswordHasher<User>()).GetAwaiter().GetResult();
}

// setup 
var completedPayment = new StripeCompletedSessionStatus();
completedPayment.OnPaymentCompleted += (sessionId) =>
{
	using (var scope = app.Services.CreateScope())
	{
		var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		var notify = scope.ServiceProvider.GetRequiredService<INotificationHubService>();
		var projects = db.Projects.Where(p => p.StripeCheckoutSessionId == sessionId);
		projects
		.ExecuteUpdate(s => s
		.SetProperty(p => p.Paid, true)
		.SetProperty(p => p.Status, ProjectStatus.Completed));

		var project = projects.First();

		if(project != null)
        {
			notify.SendNotificationToUser(
				db.ProjectMembers
				.Where(pm => pm.Role == MemberRole.Collaborator)
				.First(pm => pm.ProjectId == project.Id).UserId,
				new NotificationDto
				{
					Title = "Payment collected",
					Message = $"A payment of amount {FormatPrice(project.Price, project.Currency)} was collected for project {project.Title}",
					Type = NotificationType.Info,
					Metadata = new ResourceMetadata
					{
						ResourceId = project?.Id,
						ResourceType = ResourceType.Project,
						ProjectId = project?.Id
					}
				});
        }
	};
};

app.MapPost("/stripe/webhook", async (context) =>
{
	using var reader = new StreamReader(context.Request.Body);
	var body = await reader.ReadToEndAsync();
	var @event = JsonConvert.DeserializeObject<CompletedStripeSessionEvent>(body);
	completedPayment.CompletePayment(@event?.Data.Object.Id!, @event?.Data.Object.Status!, @event?.Type!);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
	try
	{
		await next(context);
	}
	catch (Exception e)
	{
		context.Response.StatusCode = 400;
		await context.Response.WriteAsync(e.Message);
	}
	// return Task.CompletedTask;
});

app.UseCors("SignalRCors");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/hubs/notifications").RequireAuthorization();

app.MapControllers();


app.Run();
