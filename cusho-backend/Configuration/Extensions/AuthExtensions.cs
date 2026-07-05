using System.Security.Claims;
using System.Text.Json;
using cusho.Configuration.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace cusho.Configuration.Extensions;

public static class AuthExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddAppAuth()
        {
            var keycloak = builder.Configuration.GetSection(KeycloakOptions.SectionName).Get<KeycloakOptions>() ??
                           throw new InvalidOperationException("Keycloak configuration not found");

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.MetadataAddress = keycloak.MetadataAddress;
                    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = keycloak.ValidIssuer,
                        ValidateAudience = true,
                        ValidAudience = keycloak.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        NameClaimType = "preferred_username",
                        RoleClaimType = ClaimTypes.Role,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            if (context.Principal?.Identity is not ClaimsIdentity identity)
                                return Task.CompletedTask;

                            var realmAccess = context.Principal.FindFirst("realm_access")?.Value;
                            if (realmAccess is not null)
                            {
                                using var doc = JsonDocument.Parse(realmAccess);
                                if (doc.RootElement.TryGetProperty("roles", out var roles))
                                {
                                    foreach (var role in roles.EnumerateArray())
                                        identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString()!));
                                }
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();
            return builder;
        }
    }
}
