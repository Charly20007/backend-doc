namespace Surgeon.Application.DTOs;

public class ItemCatalogoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Extra { get; set; }
}

/// <summary>Listas para los desplegables del CMS.</summary>
public class CatalogosDto
{
    public List<ItemCatalogoDto> Especialidades { get; set; } = new();
    public List<ItemCatalogoDto> CategoriasBlog { get; set; } = new();
    public List<ItemCatalogoDto> EstadosCita { get; set; } = new();
    public List<ItemCatalogoDto> Servicios { get; set; } = new();
}
