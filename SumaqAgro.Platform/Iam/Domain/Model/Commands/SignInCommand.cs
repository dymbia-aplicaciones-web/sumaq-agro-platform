namespace SumaqAgro.Platform.Iam.Domain.Model.Commands;

public sealed record SignInCommand(string Email, string Password)
{
    public override string ToString() => nameof(SignInCommand);
}
