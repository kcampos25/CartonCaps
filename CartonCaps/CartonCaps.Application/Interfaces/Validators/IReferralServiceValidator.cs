using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Interfaces.Validators
{
    public interface IReferralServiceValidator
    {
        Task<ReferralEntity> GetReferralOrThrowAsync(string referralCode);
    }
}
