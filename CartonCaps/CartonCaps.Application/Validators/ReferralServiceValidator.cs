using AutoMapper;
using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Validators;
using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Validators
{
    /// <summary>
    /// Class to centralize validations related to the referred friends process
    /// </summary>
    public class ReferralServiceValidator: IReferralServiceValidator
    {
        private readonly IReferralRepository _referralRepository;

        public ReferralServiceValidator(IReferralRepository referralRepository, IReferralVisitRepository referralVisitRepository, IMapper mapper)
        {
            _referralRepository = referralRepository;
        }

        /// <summary>
        /// Validates that the referral code exists; otherwise, throws an exception.
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns>Referral code object or an exception</returns>
        public async Task<ReferralEntity> GetReferralOrThrowAsync(string referralCode)
        {
            var referral = await _referralRepository.GetReferralByCode(referralCode);
            if (referral is null)
                throw new KeyNotFoundException("Referral code not found.");

            return referral;
        }
    }
}
