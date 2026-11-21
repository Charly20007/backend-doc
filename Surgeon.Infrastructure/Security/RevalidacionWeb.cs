using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Settings;

namespace Surgeon.Infrastructure.Security;

/// <summary>Llama a POST {Web:Url}/api/revalidate con el secreto compartido.</summary>
public class RevalidacionWeb : IRevalidacionWeb
{
    private readonly HttpClient _http;
    private readonly WebSettings _web;
    private readonly ILogger<RevalidacionWeb> _logger;

    public RevalidacionWeb(HttpClient http, IOptions<WebSettings> web, ILogger<RevalidacionWeb> logger)
    {
        _http = http;
        _web = web.Value;
        _logger = logger;
    }

    public async Task NotificarCambioAsync(CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_web.Url) || string.IsNullOrWhiteSpace(_web.RevalidateSecret))
        {
            _logger.LogDebug("Revalidación de la web desactivada (falta Web:Url o Web:RevalidateSecret).");
            return;
        }

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{_web.Url.TrimEnd('/')}/api/revalidate");
            request.Headers.Add("x-revalidate-secret", _web.RevalidateSecret);
            using var response = await _http.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("La web respondió {Status} al revalidar.", (int)response.StatusCode);
        }
        catch (Exception ex)
        {
            // La web puede estar apagada: el guardado en el CMS no debe fallar por eso
            _logger.LogWarning(ex, "No se pudo avisar a la web para que se actualice.");
        }
    }
}
