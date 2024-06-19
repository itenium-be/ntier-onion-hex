using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace N_Tier.Shared.Services.Impl
{
    /// <summary>
    /// ATTN 3: The DataAccess layer has a dependency on this Shared layer.
    ///         And this Shared layer has a dependency on the IHttpContextAccessor.
    ///
    ///         Abstraction is broken: our DataAccess layer (DatabaseContext) now
    ///         has a dependency on the Presentation layer!
    ///
    /// How to solve?
    /// Either have multiple implementations:
    /// HttpClaimService : IClaimService
    /// JobClaimService  : IClaimService
    ///
    /// Or... introduce another layer of indirection!
    /// interface IClaimsProvider {}
    /// </summary>
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return GetClaim(ClaimTypes.NameIdentifier);
        }

        public string GetClaim(string key)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value;
        }
    }
}
