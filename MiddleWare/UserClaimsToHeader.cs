using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Apartments.Middleware{
    public class UserClaimsToHeader{
        private  readonly RequestDelegate _next;
        public UserClaimsToHeader(RequestDelegate next){
            _next = next;
        }
        public async Task Invoke(HttpContext context){
            if(context.User?.Identity?.IsAuthenticated == true){
            
                var userEmail = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub || x.Type == ClaimTypes.NameIdentifier);
                if(userEmail != null){
                    
                    context.Request.Headers.Append("email", userEmail.Value);
                }
            }
            await _next(context);
        }
    }
}