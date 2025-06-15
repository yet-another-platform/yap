using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Types.Exceptions;

namespace Types.Extensions;

public static class ClaimsPrincipalExtensions
{
    private const string SubjectClaimTypeSpecificationUrl =
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        string subject = FindClaim(claimsPrincipal, JwtRegisteredClaimNames.Sub, SubjectClaimTypeSpecificationUrl)
            ?.Value ?? throw new MissingClaimException("Couldn't find user ID claim");
        return Guid.Parse(subject!);
    }

    public static Guid TryGetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        string subject =
            FindClaim(claimsPrincipal, JwtRegisteredClaimNames.Sub, SubjectClaimTypeSpecificationUrl)?.Value ??
            string.Empty;
        return Guid.TryParse(subject!, out var guid) ? guid : Guid.Empty;
    }

    private static Claim? FindClaim(ClaimsPrincipal principal, params string[] types)
    {
        var identities = principal.Identities.ToArray();
        return identities.Length == 0
            ? null
            : identities[0].Claims.FirstOrDefault(c => Array.Exists(types, t => c.Type.Equals(t)));
    }
}