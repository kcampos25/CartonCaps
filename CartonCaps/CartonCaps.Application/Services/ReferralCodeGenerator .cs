using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Services;

namespace CartonCaps.Application.Services
{
    /// <summary>
    /// Service to centralize the generation of the referral code.
    /// </summary>
    public class ReferralCodeGenerator: IReferralCodeGenerator
    {
        private readonly IReferralRepository _referralRepository;

        public ReferralCodeGenerator(IReferralRepository referralRepository)
        {
            _referralRepository = referralRepository;
        }

        /// <summary>
        /// Function that returns a unique referral code to register the invitation
        /// </summary>
        /// <param name="referralCodeBase"></param>
        /// <returns>unique referral code</returns>
        public async Task<string> GenerateUniqueReferralCodeAsync(string referralCodeBase)
        {
            string referralCode;
            do
            {
                referralCode = CreateReferralCode(referralCodeBase);
            } while (await _referralRepository.ReferralCodeExistsAsync(referralCode));

            return referralCode;
        }

        /// <summary>
        /// Internal function to generate the referral code
        /// </summary>
        /// <param name="referralCodeBase"></param>
        /// <returns>referral code</returns>
        private string CreateReferralCode(string referralCodeBase)
        {
            var random = new Random();
            return new string(Enumerable.Range(0, 7).Select(_ => referralCodeBase[random.Next(referralCodeBase.Length)]).ToArray());
        }
    }
}
