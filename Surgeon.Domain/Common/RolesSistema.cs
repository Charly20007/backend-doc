namespace Surgeon.Domain.Common;

/// <summary>Nombres de rol tal como están en dbo.Roles.</summary>
public static class RolesSistema
{
    public const string Administrador = "Administrador";
    public const string Cirujano = "Cirujano";
    public const string Secretaria = "Secretaria";

    /// <summary>Pueden editar contenido y SEO de la web.</summary>
    public const string Contenido = Administrador + "," + Cirujano;
    /// <summary>Pueden gestionar la agenda de citas.</summary>
    public const string Agenda = Administrador + "," + Cirujano + "," + Secretaria;
}
