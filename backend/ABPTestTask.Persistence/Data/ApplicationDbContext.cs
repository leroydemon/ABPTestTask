using ABPTestTask.Common.Bookings;
using ABPTestTask.Common.Equipments;
using ABPTestTask.Common.Hall;
using ABPTestTask.Common.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Hall> Halls { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Equipment> Services {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new HallConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
        }
    }
}
