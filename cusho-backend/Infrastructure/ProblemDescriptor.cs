using Microsoft.AspNetCore.Http;

namespace cusho.Infrastructure;

internal sealed record ProblemDescriptor(
    int StatusCode,
    string Title,
    string Detail,
    string Type);

internal static class ProblemDescriptors
{
    public static readonly ProblemDescriptor BadRequest = new(
        StatusCodes.Status400BadRequest,
        "Bad Request",
        "The request was invalid or cannot be processed.",
        "https://tools.ietf.org/html/rfc9110#section-15.5.1");

    public static readonly ProblemDescriptor NotFound = new(
        StatusCodes.Status404NotFound,
        "Not Found",
        "The requested resource was not found.",
        "https://tools.ietf.org/html/rfc9110#section-15.5.5");

    public static readonly ProblemDescriptor Unauthorized = new(
        StatusCodes.Status401Unauthorized,
        "Unauthorized",
        "Authentication is required to access this resource.",
        "https://tools.ietf.org/html/rfc9110#section-15.5.2");

    public static readonly ProblemDescriptor Forbidden = new(
        StatusCodes.Status403Forbidden,
        "Forbidden",
        "You do not have permission to access this resource.",
        "https://tools.ietf.org/html/rfc9110#section-15.5.4");
}