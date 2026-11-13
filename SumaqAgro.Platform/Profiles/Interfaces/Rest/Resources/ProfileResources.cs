using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;

public sealed record UpdateProfileResource(
    [param: Required, StringLength(100)] string FirstName,
    [param: Required, StringLength(100)] string LastName,
    [param: StringLength(15)] string? PhoneNumber,
    string? PhotoUrl,
    [param: RegularExpression("(?i)^(ES|EN|\\s*)$")] string? PreferredLanguage);

public sealed record RegisterCooperativeResource(
    [param: Required, RegularExpression(SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects.Ruc.Pattern)] string Ruc,
    [param: Required, StringLength(255)] string BusinessName,
    [param: Required, StringLength(255)] string Region,
    [param: Range(1, long.MaxValue)] long ManagerUserId);

public sealed record AddCooperativeMemberResource(
    [param: Required, RegularExpression(SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects.Dni.Pattern)] string Dni,
    [param: Required, StringLength(100)] string FirstName,
    [param: Required, StringLength(100)] string LastName,
    [param: Required, StringLength(100)] string Community,
    DateOnly? JoinedAt);

public sealed record InviteAgronomistResource([param: Required, StringLength(255)] string Email);

public sealed record AssignAgronomistResource(
    [param: Range(1, long.MaxValue)] long CooperativeId,
    [param: Range(1, long.MaxValue)] long AgronomistUserId,
    [param: Range(1, long.MaxValue)] long PlotId);

public sealed record ProfileResource(long Id, long UserId, string FirstName, string LastName,
    string? PhoneNumber, string? PhotoUrl, string PreferredLanguage)
{
    public static ProfileResource From(Profile profile) =>
        new(profile.Id, profile.UserId, profile.FirstName, profile.LastName,
            profile.PhoneNumber, profile.PhotoUrl, profile.PreferredLanguage);
}
