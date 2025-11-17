using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Interfaces.Repositories
{
    public interface IReferralRepository
    {
        Task<IEnumerable<UserReferralEntity>> GetReferralByUserAsync(Guid referrerUserId);
        Task CreateReferralAsync(ReferralEntity referralEntity);
        Task<bool> ReferralCodeExistsAsync(string referralCode);
        Task<ReferralEntity?> GetReferralByCode(string referralCode);
    }
}
