# Identidad y acceso

Administra el registro de usuarios y el inicio de sesión. `User` representa la cuenta y `EmailAddress` valida el correo.

## Reglas de negocio

El registro recibe correo, contraseña, un rol y datos básicos del perfil. Se admiten `ROLE_FARMER`, `ROLE_COOPERATIVE_DIRECTOR` y `ROLE_AGRONOMIST`. El correo debe ser único. La contraseña se almacena como hash BCrypt. `UserRegisteredEvent` activa la creación del perfil inicial mediante la fachada de Profiles. El inicio de sesión valida las credenciales y devuelve un JWT.

## Endpoints

| Método | Ruta | Operación |
| --- | --- | --- |
| `POST` | `/api/v1/users` | Registrar usuario y devolver `201 Created` |
| `POST` | `/api/v1/tokens` | Iniciar sesión y obtener el JWT |

## Persistencia

Entity Framework Core administra en MySQL las tablas `users`.

## Prueba local

Ambas rutas son públicas. Registra una cuenta con un correo nuevo e inicia sesión con las mismas credenciales. Usa el JWT para probar las rutas protegidas de los otros bounded.

Ejecuta las solicitudes de [iam.http](../../SumaqAgro.Platform/Requests/iam.http) desde Rider o usa [Scalar](http://localhost:5192/scalar/v1) con la API ejecutándose en el perfil local.
