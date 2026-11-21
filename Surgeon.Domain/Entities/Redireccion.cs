using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

/// <summary>
/// Redirección de una URL antigua a una nueva (301/308). Se crea automáticamente
/// cuando cambia el slug de un procedimiento o artículo, para no perder el posicionamiento.
/// </summary>
public class Redireccion : BaseEntity
{
    public string RutaOrigen { get; set; } = string.Empty;
    public string RutaDestino { get; set; } = string.Empty;
    public bool EsPermanente { get; set; } = true;
    public bool Activa { get; set; } = true;
    public string? Nota { get; set; }
}
