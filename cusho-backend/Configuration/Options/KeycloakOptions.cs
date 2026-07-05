using System.ComponentModel.DataAnnotations;

namespace cusho.Configuration.Options;

public sealed class KeycloakOptions
{
    public const string SectionName = "Keycloak";
    [Required] public required string MetadataAddress { get; init; }
    [Required] public required string ValidIssuer { get; init; }
    [Required] public required string Audience { get; init; }
}

