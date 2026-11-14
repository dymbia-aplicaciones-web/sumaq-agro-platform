# Plans, Subscriptions, and Payments

Manages the plan catalog, subscriptions for authenticated users, and simulated payment attempts. The model includes `Plan`, `Subscription`, and `Payment`.

## Business rules

`SEED` is available to `ROLE_FARMER`, `COOPERATIVE_PRO` to `ROLE_COOPERATIVE_DIRECTOR`, and `TECHNICAL_ADVISOR` to `ROLE_AGRONOMIST`. Checkout calculates the amount using the monthly or annual billing cycle. A simulated `COMPLETED` payment creates the subscription. A declined payment is recorded and can be retried. Users with an existing subscription cannot check out again.

## Endpoints

| Method | Route | Operation |
| --- | --- | --- |
| `GET` | `/api/v1/plans` | List plans |
| `GET` | `/api/v1/subscriptions/me` | Get the authenticated user's subscription |
| `POST` | `/api/v1/subscriptions/me/checkout` | Simulate payment for a plan |

## Persistence

Entity Framework Core manages these MySQL tables: `plans`, `subscriptions`, `payments`.

## Local testing

Use a JWT with the role required by your plan. Test a declined payment first, then an approved payment, and check `/api/v1/subscriptions/me`. The initial plans are seeded by a migration.

Run the [subscriptions.http](../../SumaqAgro.Platform/Requests/subscriptions.http) requests in Rider or use [Scalar](http://localhost:5192/scalar/v1) while the API is running with the local launch profile.
