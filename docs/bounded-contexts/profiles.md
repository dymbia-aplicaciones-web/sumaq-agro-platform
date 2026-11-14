# Perfiles y cooperativas

Administra perfiles, cooperativas y la relación entre cooperativas y agrónomos. El dominio incluye `Profile`, `Cooperative`, `CooperativeMember`, `AgronomistInvitation` y `AgronomistAssignment`.

## Reglas de negocio

El perfil inicial se crea a partir del registro de IAM y se consulta por ID de usuario. Los miembros y las invitaciones se agregan a una cooperativa existente. El dominio valida el DNI y el RUC mediante objetos de valor. Las asignaciones se pueden consultar por parcela o por cooperativa.

## Endpoints

| Método | Ruta | Operación |
| --- | --- | --- |
| `GET` | `/api/v1/profiles/user/{userId}` | Consultar perfil |
| `PUT` | `/api/v1/profiles/user/{userId}` | Actualizar perfil |
| `POST` | `/api/v1/cooperatives` | Registrar cooperativa |
| `POST` | `/api/v1/cooperatives/{cooperativeId}/members` | Agregar miembro |
| `POST` | `/api/v1/cooperatives/{cooperativeId}/invitations` | Registrar invitación a un agrónomo |
| `POST` | `/api/v1/agronomist-assignments` | Registrar asignación |
| `GET` | `/api/v1/agronomist-assignments/plot/{plotId}` | Consultar asignación activa por parcela |
| `GET` | `/api/v1/agronomist-assignments?cooperativeId={id}` | Listar asignaciones de una cooperativa |

## Persistencia

Entity Framework Core administra en MySQL las tablas `profiles`, `cooperatives`, `cooperative_members`, `agronomist_invitations`, `agronomist_assignments`.

## Prueba local

Usa un JWT y el ID de un usuario registrado en IAM. Para los flujos de cooperativas y asignaciones, usa los IDs de los registros correspondientes.

Ejecuta las solicitudes de [profiles.http](../../SumaqAgro.Platform/Requests/profiles.http) desde Rider o usa [Scalar](http://localhost:5192/scalar/v1) con la API ejecutándose en el perfil local.
