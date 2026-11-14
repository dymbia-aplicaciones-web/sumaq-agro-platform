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
