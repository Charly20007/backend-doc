namespace Surgeon.Application.Common;

/// <summary>
/// Un guardado violó una restricción única de la base de datos (dos operaciones simultáneas
/// sobre el mismo dato). La lanza la capa Infrastructure; los casos de uso la convierten en Error.Conflicto.
/// </summary>
public class ConflictoDeDatosException : Exception
{
    public ConflictoDeDatosException(string mensaje, Exception? interna = null) : base(mensaje, interna) { }
}
