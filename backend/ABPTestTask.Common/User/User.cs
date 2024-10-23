using Microsoft.AspNetCore.Identity;

namespace ABPTestTask.Common.User
{
    public class User : IdentityUser<Guid>
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
