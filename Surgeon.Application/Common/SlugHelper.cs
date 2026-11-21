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

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NoAlfanumerico();
}
