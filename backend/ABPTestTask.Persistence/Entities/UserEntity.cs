using ABPTestTask.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ABPTestTask.Common.User
{
    public class UserEntity : IdentityUser<Guid>, IEntity
    {
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}

