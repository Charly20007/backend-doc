using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Surgeon.Application.Common;

public static partial class SlugHelper
{
    /// <summary>"Cirugía de Vesícula (Láser)" → "cirugia-de-vesicula-laser"</summary>
    public static string Generar(string texto)
    {
        var normalizado = texto.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalizado)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        var slug = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        slug = NoAlfanumerico().Replace(slug, "-").Trim('-');
        return slug.Length > 150 ? slug[..150].Trim('-') : slug;
    }

    /// <summary>
    /// Libera un slug al eliminar un registro (la columna es única aunque el registro esté borrado):
    /// "cirugia-de-vesicula" → "cirugia-de-vesicula--eliminado-7"
    /// </summary>
    public static string MarcarEliminado(string slug, int id, int largoMaximo)
    {
        var sufijo = $"--eliminado-{id}";
        return (slug.Length + sufijo.Length > largoMaximo ? slug[..(largoMaximo - sufijo.Length)] : slug) + sufijo;
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NoAlfanumerico();
}
