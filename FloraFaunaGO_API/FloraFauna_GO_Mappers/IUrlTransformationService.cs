using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Entities2Dto;

public interface IUrlTransformationService
{
    string? TransformFileUrl(string? filename, HttpRequest request);
    string? TransformFileUrl(string? filename, string? host, string scheme);
}

public class UrlTransformationService : IUrlTransformationService
{
    public string? TransformFileUrl(string? filename, HttpRequest request)
    {
        if (string.IsNullOrEmpty(filename))
            return filename;

        var host = request.Headers["X-Forwarded-Host"].FirstOrDefault() ?? "codefirst.iut.uca.fr";
        return TransformFileUrl(filename, host, request.Scheme);
    }

    public string? TransformFileUrl(string? filename, string? host, string scheme)
    {
        if (string.IsNullOrEmpty(filename))
            return filename;

        if (Environment.GetEnvironmentVariable("TYPE") == "BDD")
        {
            var actualHost = host ?? "codefirst.iut.uca.fr";
            var basePath = "/containers/FloraFauna_GO-api";
            return $"https://{actualHost}{basePath}/api/Files/{filename}";
        }
        else
        {
            // For local development - construct URL manually since we don't have IUrlHelper access here
            return $"{scheme}://localhost:7171/api/Files/ServeImage?fileName={filename}";
        }
    }
}