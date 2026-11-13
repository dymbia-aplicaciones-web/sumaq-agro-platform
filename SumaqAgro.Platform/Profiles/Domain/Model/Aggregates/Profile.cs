using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;

public sealed class Profile(long userId, string firstName, string lastName)
{
    internal Profile(User user, string firstName, string lastName) : this(0, firstName, lastName)
    {
        User = user;
    }

    public User? User { get; private set; }
    public long Id { get; private set; }
    public long UserId { get; private set; } = userId;
    public string FirstName { get; private set; } = firstName.Trim();
    public string LastName { get; private set; } = lastName.Trim();
    public string? PhoneNumber { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string PreferredLanguage { get; private set; } = "ES";

    public void Update(string firstName, string lastName, string? phoneNumber, string? photoUrl, string? language)
    {
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
        PhotoUrl = photoUrl;
        if (!string.IsNullOrWhiteSpace(language))
            PreferredLanguage = language.ToUpperInvariant();
    }
}
