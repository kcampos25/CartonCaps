using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Domain.Entities;
using CartonCaps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Infrastructure.Repositories
{
    /// <summary>
    /// Repository responsible for managing and centralizing access to data
    /// related to the recording of views or clicks on links sent by friends.
    /// </summary>
    public class ReferralVisitRepository : IReferralVisitRepository
    {
        private readonly CartonCapsContext _context;
        public ReferralVisitRepository(CartonCapsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Record each visit to the link, that is, each time the
        /// user to whom the link is sent clicks on it.
        /// </summary>
        /// <param name="referralVisitEntity"></param>
        public async Task CreateReferralVisit(ReferralVisitEntity referralVisitEntity)
        {
            var referral = _context.Referrals.FirstOrDefault(r => r.Id == referralVisitEntity.ReferralId);

            if (referral != null)
            {
                var visit = new ReferralVisit
                {
                    Id = Guid.NewGuid(),
                    VisitedAt = referralVisitEntity.VisitedAt,
                    IpAddress = referralVisitEntity.IpAddress,
                    ReferralId = referralVisitEntity.ReferralId
                };

                _context.ReferralVisits.Add(visit);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Function responsible for listing all records of visits or clicks on links.
        /// </summary>
        /// <returns>list of registered visits</returns>
        public async Task<IEnumerable<ReferralVisitHistoryEntity>> GetReferralVisitHistoryAsync()
        {
            return await _context.ReferralVisits
                        .OrderByDescending(visit => visit.VisitedAt)
                        .Select(visit => new ReferralVisitHistoryEntity
                        {
                            VisitedAt = visit.VisitedAt,
                            IpAddress = visit.IpAddress,
                            ReferrerUser = visit.Referral.ReferrerUser.FirstName + " " + visit.Referral.ReferrerUser.LastName

                        }).ToListAsync();
        }


        /// <summary>
        /// Returns the last recorded click for a referral and IP address within the defined time interval
        /// </summary>
        /// <returns>Object containing information about a user’s click on a referral link</returns>
        public async Task<ReferralVisitEntity?> GetLastClickByIpAsync(Guid referralId, string ipAddress, TimeSpan window)
        {
            var cutoff = DateTime.UtcNow - window;

            return await _context.ReferralVisits
                .Where(visit => visit.ReferralId == referralId && visit.IpAddress == ipAddress && visit.VisitedAt >= cutoff)
                .OrderByDescending(visit => visit.VisitedAt)
                 .Select(visit => new ReferralVisitEntity
                 {
                     Id = visit.Id,
                     VisitedAt = visit.VisitedAt,
                     IpAddress = visit.IpAddress,
                     ReferralId = visit.ReferralId
                 })
                .FirstOrDefaultAsync();
        }
    }
}
