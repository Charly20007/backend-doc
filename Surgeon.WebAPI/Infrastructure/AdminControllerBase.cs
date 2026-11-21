using Microsoft.AspNetCore.Authorization;
using Surgeon.Domain.Common;

namespace Surgeon.WebAPI.Infrastructure;

/// <summary>Base de los controladores del CMS: exige sesión con rol de contenido (Administrador o Cirujano).</summary>
[Authorize(Roles = RolesSistema.Contenido)]
public abstract class AdminControllerBase : ApiControllerBase
{
}
