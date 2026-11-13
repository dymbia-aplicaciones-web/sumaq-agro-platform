namespace SumaqAgro.Platform.Profiles.Domain.Model.Commands;

public sealed record UpdateProfileCommand(long UserId, string FirstName, string LastName, string? PhoneNumber, string? PhotoUrl, string? Language);
