﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation
{
  public class IdentityServerStaticConfig
  {
    public const string ClientId = "snkrs-monitors-management-api";
    public const string ClientSecret = "SECRET:snkrs-monitors-management-api";
    private const string ClientName = "SNKRS Monitors Management API";

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
      yield return new IdentityResources.OpenId();
      yield return new IdentityResources.Profile();
      yield return new IdentityResource("olympus-api", "Identity ID", new[] {JwtClaimTypes.Id});
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
      yield return new ApiResource(ClientId, ClientName)
      {
        Enabled = true,
        UserClaims =
        {
          JwtClaimTypes.Id,
          JwtClaimTypes.Role,
          JwtClaimTypes.Email,
          JwtClaimTypes.EmailVerified,
          JwtClaimTypes.FamilyName,
          JwtClaimTypes.GivenName,
          JwtClaimTypes.BirthDate,
          JwtClaimTypes.Name,
          JwtClaimTypes.Picture,
          JwtClaimTypes.Expiration,
          JwtClaimTypes.IssuedAt,
          JwtClaimTypes.Issuer,
          JwtClaimTypes.PhoneNumber,
          JwtClaimTypes.PhoneNumberVerified,
          // JwtClaimTypes.Audience,
          ClaimTypes.Name,
          ClaimTypes.Role,
        },
        Scopes =
        {
          IdentityServerConstants.StandardScopes.OfflineAccess,
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Profile,
          ClientId,
        }
      };
    }

    public static IEnumerable<Client> GetClients()
    {
      yield return new Client
      {
        Enabled = true,
        ClientId = ClientId,
        ClientName = ClientName,

        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
        RefreshTokenExpiration = TokenExpiration.Sliding,
        SlidingRefreshTokenLifetime = (int) TimeSpan.FromHours(1).TotalSeconds,
        AccessTokenLifetime = (int) TimeSpan.FromMinutes(5).TotalSeconds,
        ClientSecrets = {new Secret(ClientSecret.Sha256())},
        RefreshTokenUsage = TokenUsage.OneTimeOnly,
        AllowedScopes =
        {
          IdentityServerConstants.StandardScopes.OfflineAccess,
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Profile,
          ClientId,
        },
        AlwaysSendClientClaims = true,
        AllowOfflineAccess = true,
        AlwaysIncludeUserClaimsInIdToken = true,
      };
    }

    public static List<TestUser> GetUsers()
    {
      return new List<TestUser>
      {
        new TestUser
        {
          Username = "admin@snkrs-monitors.projectinduscries.gg",
          Password = "admin@snkrs-monitors.projectinduscries.gg",
          IsActive = true,
          SubjectId = "1"
        }
      };
    }
  }
}