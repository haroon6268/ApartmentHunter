namespace Apartments.Middleware{
    public class JwtTokenCookieMiddleware{
        private readonly RequestDelegate _next;
        public JwtTokenCookieMiddleware(RequestDelegate next){
            _next = next;
        }

        public async Task Invoke(HttpContext context){
            var tokenCookieName = "AuthToken";
            if(context.Request.Cookies.ContainsKey(tokenCookieName)){
                var token = context.Request.Cookies[tokenCookieName];
                if(!string.IsNullOrEmpty(token)){
                    context.Request.Headers.Append("Authorization", $"Bearer {token}");
                    Console.WriteLine("hERE");
                }
            }
            await _next(context);
        }

    

    }
}