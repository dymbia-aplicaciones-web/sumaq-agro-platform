# Profiles and Cooperatives

Manages user profiles, cooperatives, and their relationships with agronomists. The model includes `Profile`, `Cooperative`, `CooperativeMember`, `AgronomistInvitation`, and `AgronomistAssignment`.

## Business rules

IAM registration creates the initial profile, which can be retrieved by user ID. Memberships and invitations belong to an existing cooperative. Value objects validate Peruvian national ID (DNI) and taxpayer registration (RUC) numbers. Assignments can be queried by plot or cooperative.

## Endpoints

| Method | Route | Operation |
| --- | --- | --- |
| `GET` | `/api/v1/profiles/user/{userId}` | Get a profile |
| `PUT` | `/api/v1/profiles/user/{userId}` | Update a profile |
| `POST` | `/api/v1/cooperatives` | Register a cooperative |
| `POST` | `/api/v1/cooperatives/{cooperativeId}/members` | Add a member |
| `POST` | `/api/v1/cooperatives/{cooperativeId}/invitations` | Record an agronomist invitation |
| `POST` | `/api/v1/agronomist-assignments` | Create an assignment |
| `GET` | `/api/v1/agronomist-assignments/plot/{plotId}` | Get the active assignment for a plot |
| `GET` | `/api/v1/agronomist-assignments?cooperativeId={id}` | List assignments by cooperative |

## Persistence

Entity Framework Core manages these MySQL tables: `profiles`, `cooperatives`, `cooperative_members`, `agronomist_invitations`, `agronomist_assignments`.

## Local testing

Use a JWT and the ID of a user registered through IAM. Use the corresponding record IDs to test cooperative and assignment workflows.

Run the [profiles.http](../../SumaqAgro.Platform/Requests/profiles.http) requests in Rider or use [Scalar](http://localhost:5192/scalar/v1) while the API is running with the local launch profile.
