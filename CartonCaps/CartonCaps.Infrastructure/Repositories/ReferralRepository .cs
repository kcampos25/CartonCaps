using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Domain.Entities;
using CartonCaps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace CartonCaps.Infrastructure.Repositories
{
    /// <summary>
    /// Repository responsible for managing and centralizing access to data from the application's referrals module.
    /// </summary>
    public class ReferralRepository : IReferralRepository
    {
        private readonly CartonCapsContext _context;
        public ReferralRepository(CartonCapsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function responsible for obtaining the people invited by a user (my referrals).
        /// </summary>
        /// <param name="referrerUserId"></param>
        /// <returns>List of a user's referrals(name,status)</returns>
        public async Task<IEnumerable<UserReferralEntity>> GetReferralByUserAsync(Guid referrerUserId)
        {
            return await _context.Referrals
                .Where(r => r.ReferrerUserId == referrerUserId && r.Status != "Pending")
                .Select(r => new UserReferralEntity
                {
                    InviteeName = r.InviteName,
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Function responsible for creating a new record in the referral table, that is, 
        /// it registers the invitation in pending status
        /// </summary>
        /// <param name="referralEntity"></param>
        public async Task CreateReferralAsync(ReferralEntity referralEntity)
        {
            var referral = new Referral
            {
                Id = referralEntity.Id != Guid.Empty ? referralEntity.Id : Guid.NewGuid(),
                InvitationCode = referralEntity.InvitationCode,
                InviteName = referralEntity.InviteName,
                LinkGenerated = referralEntity.LinkGenerated,
                Status = referralEntity.Status ?? "Pending",
                CreatedAt = referralEntity.CreatedAt != default ? referralEntity.CreatedAt : DateTime.UtcNow,
                ReferrerUserId = referralEntity.ReferrerUserId
            };

            await _context.Referrals.AddAsync(referral);
           await  _context.SaveChangesAsync();
        }

        /// <summary>
        /// Function that checks if a referral code exists for a specific user
        /// </summary>
        /// <param name="referrerUserId"></param>
        /// <param name="referralCode"></param>
        /// <returns>depending on whether the referral code exists or not (true/false)</returns>
        public async Task<bool> ReferralCodeExistsAsync(Guid referrerUserId, string referralCode)
        {
            return await _context.Referrals
                .AnyAsync(r => r.ReferrerUserId == referrerUserId && r.InvitationCode == referralCode);
        }
    }
}
