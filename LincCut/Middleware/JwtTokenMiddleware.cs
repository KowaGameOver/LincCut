namespace LincCut.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("jwtToken", out var jwtToken))
                context.Request.Headers.Add("Authorization", $"Bearer {jwtToken}");
            await _next(context);
        }
    }
}
