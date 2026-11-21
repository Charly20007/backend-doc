namespace Surgeon.Application.Settings;

public class JwtSettings
{
    public const string Seccion = "Jwt";

    /// <summary>Clave secreta HMAC (mínimo 32 caracteres). En producción, por variable de entorno Jwt__Key.</summary>
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "Surgeon.WebAPI";
    public string Audience { get; set; } = "Surgeon.CMS";
    public int ExpiracionHoras { get; set; } = 8;
}

/// <summary>
/// Usuario administrador que la API crea al iniciar si la tabla Usuarios está vacía.
/// </summary>
public class AdminInicialSettings
{
    public const string Seccion = "AdminInicial";

    public string? NombreCompleto { get; set; }
    public string? Correo { get; set; }
    public string? Password { get; set; }
}

/// <summary>Web pública que se debe refrescar al guardar en el CMS.</summary>
public class WebSettings
{
    public const string Seccion = "Web";

    /// <summary>URL de la web, ej. https://cirujanogeneral.pe</summary>
    public string? Url { get; set; }

    /// <summary>Debe coincidir con REVALIDATE_SECRET de la web.</summary>
    public string? RevalidateSecret { get; set; }
}
