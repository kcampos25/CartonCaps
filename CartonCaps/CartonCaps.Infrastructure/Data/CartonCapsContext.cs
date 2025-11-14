using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Infrastructure.Data
{
    public class CartonCapsContext : DbContext
    {
        public CartonCapsContext(DbContextOptions<CartonCapsContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Referral> Referrals { get; set; }     
        public DbSet<ReferralVisit> ReferralVisits { get; set; } //Record clicks on referral links for abuse control

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Users -> Referrer (user who recommended)
            modelBuilder.Entity<User>()
                .HasOne(u => u.ReferredBy)
                .WithMany()
                .HasForeignKey(u => u.ReferredById)
                .OnDelete(DeleteBehavior.Restrict);

            //ReferralClick -> Referral
            modelBuilder.Entity<ReferralVisit>()
                .HasOne(rc => rc.Referral)
                .WithMany(r => r.Visits)
                .HasForeignKey(rc => rc.ReferralId);

            //Unique InvitationCode per user
            modelBuilder.Entity<Referral>()
                .HasIndex(r => new { r.ReferrerUserId, r.InvitationCode })
                .IsUnique();
        }
    }
}
