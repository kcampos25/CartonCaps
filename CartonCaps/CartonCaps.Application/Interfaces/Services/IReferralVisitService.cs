using CartonCaps.Application.DTOs;

namespace CartonCaps.Application.Interfaces.Services
{
    public interface IReferralVisitService
    {
        Task<ReferralVisitResponse> CreateReferralVisit(ReferralRedirectRequest referralRedirectRequest);
        Task<IEnumerable<ReferralVisitHistoryResponse>> GetReferralVisitHistoryAsync();
    }
}
