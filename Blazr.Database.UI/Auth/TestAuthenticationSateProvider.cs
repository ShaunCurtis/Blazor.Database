/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blazr.Database.UI
{
    public class TestAuthenticationStateProvider : AuthenticationStateProvider
    {

        public TestUserType UserType { get; private set; } = TestUserType.None;

        private ClaimsPrincipal Admin
        {
            get
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Sid, "985fdabb-5e4e-4637-b53a-d331a3158680"),
                    new Claim(ClaimTypes.Name, "Administrator"),
                    new Claim(ClaimTypes.Role, "Admin")
                }, "Test authentication type");
                return new ClaimsPrincipal(identity);
            }
        }

        private ClaimsPrincipal User
        {
            get
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Sid, "024672e0-250a-46fc-bd35-1902974cf9e1"),
                    new Claim(ClaimTypes.Name, "Normal User"),
                    new Claim(ClaimTypes.Role, "User")
                }, "Test authentication type");
                return new ClaimsPrincipal(identity);
            }
        }

        private ClaimsPrincipal Visitor
        {
            get
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Sid, "3ef75379-69d6-4f8b-ab5f-857c32775571"),
                    new Claim(ClaimTypes.Name, "Visitor"),
                    new Claim(ClaimTypes.Role, "Visitor")
                }, "Test authentication type");
                return new ClaimsPrincipal(identity);
            }
        }

        private ClaimsPrincipal Anonymous
        {
            get
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Sid, "0ade1e94-b50e-46cc-b5f1-319a96a6d92f"),
                    new Claim(ClaimTypes.Name, "Anonymous"),
                    new Claim(ClaimTypes.Role, "Anonymous")
                }, null);
                return new ClaimsPrincipal(identity);
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var task = this.UserType switch
            {
                TestUserType.Admin => Task.FromResult(new AuthenticationState(this.Admin)),
                TestUserType.User => Task.FromResult(new AuthenticationState(this.User)),
                TestUserType.None => Task.FromResult(new AuthenticationState(this.Anonymous)),
                _ => Task.FromResult(new AuthenticationState(this.Visitor))
            };
            return task;
        }

        public Task<AuthenticationState> ChangeUser(TestUserType userType)
        {
            this.UserType = userType;
            var task = this.GetAuthenticationStateAsync();
            this.NotifyAuthenticationStateChanged(task);
            return task;
        }
    }
}