using CartonCaps.Domain.Entities;


namespace CartonCaps.Application.Interfaces.Repositories
{
    public interface IReferralVisitRepository
    {
        Task CreateReferralVisit(ReferralVisitEntity referralVisitEntity);
        Task<IEnumerable<ReferralVisitHistoryEntity>> GetReferralVisitHistoryAsync();
        Task<ReferralVisitEntity?> GetLastClickByIpAsync(Guid referralId, string ipAddress, TimeSpan window);
    }
}
