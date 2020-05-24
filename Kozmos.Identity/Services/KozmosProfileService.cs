using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Kozmos.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kozmos.Identity.Services
{
    public class KozmosProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<KozmosUser> _claimsFactory;
        private readonly UserManager<KozmosUser> _userManager;

        public KozmosProfileService(UserManager<KozmosUser> userManager, IUserClaimsPrincipalFactory<KozmosUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.Identity.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            claims.Add(new Claim("kasi", user.City ?? string.Empty));
            claims.Add(new Claim("employee_id", user.EmployeeId ?? string.Empty));


            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }

}

