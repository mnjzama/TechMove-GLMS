using System.Net.Http.Headers;

namespace TechMoveClient.Services.Api
{
    public class JwtAuthHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /*
        Author: Kamlesh Singh
        URL: https://medium.com/@kamlesh90/delegatinghandler-in-dotnet-core-d2e2a66c0eee
        Date: 19 May 2025
        Date Accessed: 10 May 2026
        */

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Retrieve JWT token from session for authenticated API requests
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JwtToken");

            // Attach token to Authorization header if it exists
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            // Continue request pipeline
            return base.SendAsync(request, cancellationToken);
        }
    }
}