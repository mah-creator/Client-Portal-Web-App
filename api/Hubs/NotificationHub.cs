namespace ClientPortalApi.Hubs;

using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

public class NotificationHub : Hub
{
	private static readonly ConcurrentDictionary<string, UserConnection> _userConnections = new();
	private readonly ILogger<NotificationHub> _logger;

	public NotificationHub(ILogger<NotificationHub> logger)
	{
		_logger = logger;
	}

	// Called when a client connects
	public override async Task OnConnectedAsync()
	{
		var userId = GetUserIdFromContext();
		if (string.IsNullOrEmpty(userId))
		{
			_logger.LogWarning("Connection attempted without user ID: {ConnectionId}", Context.ConnectionId);
			return;
		}

		var userConnection = new UserConnection
		{
			UserId = userId,
			ConnectionId = Context.ConnectionId,
			ConnectedAt = DateTime.UtcNow
		};

		_userConnections[Context.ConnectionId] = userConnection;

		// Add connection to user group
		await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");

		_logger.LogInformation("User {UserId} connected with connection ID: {ConnectionId}", userId, Context.ConnectionId);

		await base.OnConnectedAsync();
	}

	// Called when a client disconnects
	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		if (_userConnections.TryRemove(Context.ConnectionId, out var userConnection))
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userConnection.UserId}");
			_logger.LogInformation("User {UserId} disconnected. Connection ID: {ConnectionId}",
				userConnection.UserId, Context.ConnectionId);
		}

		await base.OnDisconnectedAsync(exception);
	}

	// Client can explicitly join additional groups
	public async Task JoinGroup(string groupName)
	{
		var userId = GetUserIdFromContext();
		if (string.IsNullOrEmpty(userId))
		{
			await Clients.Caller.SendAsync("Error", "Authentication required");
			return;
		}

		await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
		_logger.LogInformation("User {UserId} joined group: {GroupName}", userId, groupName);

		await Clients.Caller.SendAsync("GroupJoined", groupName);
	}

	// Client can leave groups
	public async Task LeaveGroup(string groupName)
	{
		var userId = GetUserIdFromContext();
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
		_logger.LogInformation("User {UserId} left group: {GroupName}", userId, groupName);

		await Clients.Caller.SendAsync("GroupLeft", groupName);
	}

	// Heartbeat to keep connection alive and track active users
	public async Task Heartbeat()
	{
		if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
		{
			userConnection.LastHeartbeat = DateTime.UtcNow;
			await Clients.Caller.SendAsync("HeartbeatAck", DateTime.UtcNow);
		}
	}

	// Get connection statistics (admin only)
	public async Task GetConnectionStats()
	{
		var userId = GetUserIdFromContext();

		var stats = new ConnectionStats
		{
			TotalConnections = _userConnections.Count,
			UniqueUsers = _userConnections.Values.Select(uc => uc.UserId).Distinct().Count(),
			YourConnectionId = Context.ConnectionId
		};

		await Clients.Caller.SendAsync("ConnectionStats", stats);
	}

	private string? GetUserIdFromContext()
	{
		// In SignalR, you can access the user from Context.User
		return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
	}

	public static IReadOnlyCollection<UserConnection> GetActiveConnections()
	{
		return _userConnections.Values.ToList().AsReadOnly();
	}

	public static int GetConnectionCountForUser(string userId)
	{
		return _userConnections.Values.Count(uc => uc.UserId == userId);
	}
}

// Supporting Models
public class UserConnection
{
	public string UserId { get; set; } = string.Empty;
	public string ConnectionId { get; set; } = string.Empty;
	public DateTime ConnectedAt { get; set; }
	public DateTime? LastHeartbeat { get; set; }
}

public class ConnectionStats
{
	public int TotalConnections { get; set; }
	public int UniqueUsers { get; set; }
	public string YourConnectionId { get; set; } = string.Empty;
}