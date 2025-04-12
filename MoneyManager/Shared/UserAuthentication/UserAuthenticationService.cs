using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace MoneyManager.Shared.UserAuthentication
{
    public class UserAuthenticationService : IUserAuthentication
    {
        private readonly AuthenticationStateProvider _authenticationStateAsync;

        public UserAuthenticationService(AuthenticationStateProvider authenticationStateAsync)
        {
            this._authenticationStateAsync = authenticationStateAsync;
        }

        public string? GetCurrentUserId()
        {
            try
            {
                var authState = _authenticationStateAsync.GetAuthenticationStateAsync();
                var user = authState.Result.User;

                if (user.Identity?.IsAuthenticated ?? false)
                {
                    //Depending on the environment, development or production, gets the oid accordingly
                    //https://learn.microsoft.com/en-us/entra/identity-platform/id-token-claims-reference?utm_source=chatgpt.com#use-claims-to-reliably-identify-a-user
                    string? userId = user.FindFirst("oid")?.Value
                                     ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    return userId;
                }
                else
                {
                    Console.WriteLine("User is not authenticated.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving UserId: {ex.Message}");
                throw;
            }
        }
    }
}
