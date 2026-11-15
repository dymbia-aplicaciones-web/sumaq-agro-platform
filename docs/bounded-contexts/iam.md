# Identity and Access Management

Handles user registration and sign-in. `User` represents an account and `EmailAddress` validates its email address.

## Business rules

Registration accepts an email address, a password, one role, and basic profile details. Supported roles are `ROLE_FARMER`, `ROLE_COOPERATIVE_DIRECTOR`, and `ROLE_AGRONOMIST`. Email addresses must be unique. Passwords are stored as BCrypt hashes. `UserRegisteredEvent` triggers profile creation through the Profiles facade. Signing in validates credentials and returns a JWT.

## Endpoints

| Method | Route | Operation |
| --- | --- | --- |
| `POST` | `/api/v1/users` | Register a user and return `201 Created` |
| `POST` | `/api/v1/tokens` | Sign in and obtain a JWT |

## Persistence

Entity Framework Core manages these MySQL tables: `users`.

## Local testing

Both routes are public. Register with a new email address, sign in with the same credentials, and use the JWT to test protected endpoints.

Run the [iam.http](../../SumaqAgro.Platform/Requests/iam.http) requests in Rider or use [Scalar](http://localhost:5192/scalar/v1) while the API is running with the local launch profile.

## Password recovery

- POST /api/v1/authentication/password-recovery/request accepts an email and returns 202 for both registered and unregistered addresses.
- POST /api/v1/authentication/password-recovery/reset accepts token and newPassword. A successful reset returns 204. Invalid, expired, or already used tokens return 400.
- Reset tokens expire after 15 minutes. MySQL stores their SHA-256 digests in password_reset_tokens. Passwords continue to use BCrypt.
- Configure Iam:PasswordRecovery:Brevo:ApiKey, Iam:PasswordRecovery:Brevo:SenderEmail, and Iam:PasswordRecovery:ResetUrl through User Secrets or deployment configuration. Use a verified sender and an HTTPS frontend reset page. The link adds the token as a query parameter.
- Use the token received by email as passwordResetToken when testing the reset request in Rider.
