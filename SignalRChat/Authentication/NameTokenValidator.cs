namespace SignalRChat.Authentication
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.IdentityModel.Tokens;
    
    public class NameTokenValidator : ISecurityTokenValidator
    {
        public bool CanValidateToken { get; } = true;
        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(securityToken) as JwtSecurityToken;
            var name = jwtToken.Claims.FirstOrDefault().Value;

            return new ClaimsPrincipal(new List<ClaimsIdentity>
            {
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, name)
                })
            });
        }
    }
}