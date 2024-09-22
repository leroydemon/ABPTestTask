using Microsoft.AspNetCore.Identity;

namespace BussinesLogic.EntitiesDto
{
    public class UserDto : IdentityUser<Guid>
    {
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
