# Planes, suscripciones y pagos

Administra el catálogo de planes, la suscripción del usuario autenticado y los intentos de pago de demostración. El modelo contiene `Plan`, `Subscription` y `Payment`.

## Reglas de negocio

El plan `SEED` corresponde a `ROLE_FARMER`, `COOPERATIVE_PRO` a `ROLE_COOPERATIVE_DIRECTOR` y `TECHNICAL_ADVISOR` a `ROLE_AGRONOMIST`. El checkout usa el ciclo mensual o anual para calcular el importe. Un pago simulado `COMPLETED` crea la suscripción. El pago rechazado se guarda como intento y permite reintentar. El checkout impide contratar si el usuario ya tiene una suscripción registrada.

## Endpoints

| Método | Ruta | Operación |
| --- | --- | --- |
| `GET` | `/api/v1/plans` | Listar planes |
| `GET` | `/api/v1/subscriptions/me` | Consultar la suscripción del usuario autenticado |
| `POST` | `/api/v1/subscriptions/me/checkout` | Simular el pago de un plan |

## Persistencia

Entity Framework Core administra en MySQL las tablas `plans`, `subscriptions`, `payments`.

## Prueba local

Usa un JWT con el rol del plan. Prueba primero el pago rechazado, luego el aprobado y consulta `/api/v1/subscriptions/me`. Los planes iniciales se insertan mediante una migración.

Ejecuta las solicitudes de [subscriptions.http](../../SumaqAgro.Platform/Requests/subscriptions.http) desde Rider o usa [Scalar](http://localhost:5192/scalar/v1) con la API ejecutándose en el perfil local.
