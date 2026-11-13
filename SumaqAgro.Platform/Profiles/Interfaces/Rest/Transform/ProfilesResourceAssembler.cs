using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Commands;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;
using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;

namespace SumaqAgro.Platform.Profiles.Interfaces.Rest.Transform;

internal static class ProfilesResourceAssembler
{
    public static UpdateProfileCommand ToCommand(long userId, UpdateProfileResource resource) =>
        new(userId, resource.FirstName, resource.LastName, resource.PhoneNumber, resource.PhotoUrl, resource.PreferredLanguage);

    public static RegisterCooperativeCommand ToCommand(RegisterCooperativeResource resource) =>
        new(new Ruc(resource.Ruc), resource.BusinessName, resource.Region, resource.ManagerUserId);

    public static AddCooperativeMemberCommand ToCommand(long cooperativeId, AddCooperativeMemberResource resource) =>
        new(cooperativeId, new Dni(resource.Dni), resource.FirstName, resource.LastName, resource.Community, resource.JoinedAt);

    public static InviteAgronomistCommand ToCommand(long cooperativeId, InviteAgronomistResource resource) =>
        new(cooperativeId, resource.Email);

    public static AssignAgronomistCommand ToCommand(AssignAgronomistResource resource) =>
        new(resource.CooperativeId, resource.AgronomistUserId, resource.PlotId);

    public static ProfileResource ToResource(Profile model) =>
        new(model.Id, model.UserId, model.FirstName, model.LastName,
            model.PhoneNumber, model.PhotoUrl, model.PreferredLanguage);

    public static CooperativeResource ToResource(Cooperative model) =>
        new(model.Id, model.Ruc, model.BusinessName, model.Region, model.ManagerUserId);

    public static CooperativeMemberResource ToResource(CooperativeMember model) =>
        new(model.Id, model.CooperativeId, model.Dni, model.FirstName, model.LastName, model.Community, model.JoinedAt);

    public static AgronomistInvitationResource ToResource(AgronomistInvitation model) =>
        new(model.Id, model.CooperativeId, model.Email, model.Status, model.SentAt);

    public static AgronomistAssignmentResource ToResource(AgronomistAssignment model) =>
        new(model.Id, model.CooperativeId, model.AgronomistUserId, model.PlotId, model.AssignedAt, model.Active);
}
