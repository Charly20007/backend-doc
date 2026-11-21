namespace Surgeon.Application.Common;

/// <summary>Tipos de error de negocio. La capa WebAPI los traduce a códigos HTTP.</summary>
public enum TipoError
{
    Validacion,   // 400
    NoAutorizado, // 401
    NoEncontrado, // 404
    Conflicto,    // 409
}

public sealed record Error(TipoError Tipo, string Mensaje)
{
    public static Error Validacion(string mensaje) => new(TipoError.Validacion, mensaje);
    public static Error NoAutorizado(string mensaje) => new(TipoError.NoAutorizado, mensaje);
    public static Error NoEncontrado(string mensaje) => new(TipoError.NoEncontrado, mensaje);
    public static Error Conflicto(string mensaje) => new(TipoError.Conflicto, mensaje);
}

/// <summary>
/// Resultado de un caso de uso: éxito o un <see cref="Error"/> de negocio.
/// Evita usar excepciones para situaciones esperadas (no encontrado, duplicado...).
/// </summary>
public class Resultado
{
    protected Resultado(Error? error) => Error = error;

    public Error? Error { get; }
    public bool Exito => Error is null;

    public static Resultado Ok() => new(null);
    public static Resultado Falla(Error error) => new(error);

    public static implicit operator Resultado(Error error) => Falla(error);
}

public sealed class Resultado<T> : Resultado
{
    private Resultado(T? valor, Error? error) : base(error) => Valor = valor;

    public T? Valor { get; }

    public static Resultado<T> Ok(T valor) => new(valor, null);
    public static new Resultado<T> Falla(Error error) => new(default, error);

    public static implicit operator Resultado<T>(T valor) => Ok(valor);
    public static implicit operator Resultado<T>(Error error) => Falla(error);
}
