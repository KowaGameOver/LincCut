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
            // Проверяем, есть ли JWT-токен в cookies
            if (context.Request.Cookies.TryGetValue("jwtToken", out var jwtToken))
            {
                // Устанавливаем JWT-токен в заголовки HTTP-запроса
                context.Request.Headers.Add("Authorization", $"Bearer {jwtToken}");
            }
            await _next(context);
        }
    }
}
