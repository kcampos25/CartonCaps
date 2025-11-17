using CartonCaps.Application.DTOs;

namespace CartonCaps.Application.Interfaces.Services
{
    public interface IReferralService
    {
        Task<IEnumerable<UserReferralResponse>> GetReferralByUserAsync(Guid referrerUserId);
        Task<CreateReferralInviteResponse> CreateReferralAsync(CreateReferralInviteRequest referralInviteRequest, string baseLink, string referralCodeBase);
    }
}
