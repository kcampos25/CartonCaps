using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Domain.Entities;
using CartonCaps.Infrastructure.Data;

namespace CartonCaps.Infrastructure.Repositories
{
    /// <summary>
    /// Repository to centralize access to data for app users
    /// </summary>
    public class UserRepository: IUserRepository
    {
        private readonly CartonCapsContext _context;
        public UserRepository(CartonCapsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function to retrieve information about the user who generated the referral.
        /// </summary>
        /// <param name="referrerUserId"></param>
        /// <returns>Object with user information</returns>
        public async Task<UserEntity?> GetUserById(Guid referrerUserId)
        {
            if (referrerUserId == Guid.Empty)
                return null;

            var user = await _context.Users.FindAsync(referrerUserId);

            return user == null ? null : new UserEntity
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday= user.Birthday,
                CreatedAt = user.CreatedAt,
                ReferralCode= user.ReferralCode
            };
        }
    }
}
