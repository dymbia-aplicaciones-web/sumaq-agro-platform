namespace SumaqAgro.Platform.Iam.Domain.Model.Commands;

public sealed record RegisterUserCommand(string Email, string Password, string Role, string FirstName, string LastName)
{
    public override string ToString() => nameof(RegisterUserCommand);
}
