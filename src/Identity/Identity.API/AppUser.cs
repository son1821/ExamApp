

using Microsoft.AspNetCore.Identity;

namespace Identity.API
{
    public class AppUser : IdentityUser
    {
        public string FirstName {  get; set; } = String.Empty;

        public string LastName { get; set; } = String.Empty;
    }
}
