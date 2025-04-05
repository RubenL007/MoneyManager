using Microsoft.AspNetCore.Components.Authorization;

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
                var authState =  _authenticationStateAsync.GetAuthenticationStateAsync();

                if (authState.Result.User.Identity?.IsAuthenticated ?? false)
                {
                    return authState.Result.User.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
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
