using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.Common;

namespace Surgeon.WebAPI.Infrastructure;

/// <summary>
/// Base de todos los controladores. Su única tarea es traducir el <see cref="Resultado"/>
/// de un caso de uso a una respuesta HTTP. Los controladores no contienen lógica de negocio.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>200 con el valor, o el código de error correspondiente.</summary>
    protected ActionResult<T> Responder<T>(Resultado<T> resultado) =>
        resultado.Exito ? Ok(resultado.Valor) : Fallo(resultado.Error!);

    /// <summary>201 Created con la ubicación del recurso nuevo.</summary>
    protected ActionResult<T> Creado<T>(Resultado<T> resultado, string accion, Func<T, object> ruta) =>
        resultado.Exito ? CreatedAtAction(accion, ruta(resultado.Valor!), resultado.Valor) : Fallo(resultado.Error!);

    /// <summary>204 sin contenido, o el código de error correspondiente.</summary>
    protected ActionResult SinContenido(Resultado resultado) =>
        resultado.Exito ? NoContent() : Fallo(resultado.Error!);

    /// <summary>Errores de negocio → HTTP, siempre con el cuerpo { mensaje } que muestra el CMS.</summary>
    private ObjectResult Fallo(Error error)
    {
        var codigo = error.Tipo switch
        {
            TipoError.Validacion => StatusCodes.Status400BadRequest,
            TipoError.NoAutorizado => StatusCodes.Status401Unauthorized,
            TipoError.NoEncontrado => StatusCodes.Status404NotFound,
            TipoError.Conflicto => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };
        return StatusCode(codigo, new { mensaje = error.Mensaje });
    }
}
