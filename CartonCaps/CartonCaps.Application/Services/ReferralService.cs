using AutoMapper;
using CartonCaps.Application.DTOs;
using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Services;
using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Services
{
    /// <summary>
    /// Service responsible for managing the functionalities of the App's referral module
    /// </summary>
    public class ReferralService: IReferralService
    {
        private readonly IReferralRepository _referralRepository;
        private readonly IMapper _mapper;

        public ReferralService(IReferralRepository referralRepository, IMapper mapper)
        {
            _referralRepository = referralRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Function responsible for obtaining the people invited by a user (my referrals).
        /// </summary>
        /// <param name="referrerUserId"></param>
        /// <returns>List of a user's referrals(name,status)</returns>
        public async Task<IEnumerable<UserReferralResponse>> GetReferralByUserAsync(Guid referrerUserId)
        {
            var invites = await _referralRepository.GetReferralByUserAsync(referrerUserId);
            return _mapper.Map<IEnumerable<UserReferralResponse>>(invites);
        }

        /// <summary>
        /// This function performs the following:
        ///     1- Generates a referral code.
        ///     2- Builds the invitation link.
        ///     3- Saves the invitation record in the referrals table.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="baseLink"></param>
        /// <param name="referralCodeBase"></param>
        /// <returns>Object containing the invitation link and referral code</returns>
        public async Task<CreateReferralInviteResponse> CreateReferralAsync(CreateReferralInviteRequest request, string baseLink, string referralCodeBase)
        {
            //Generate unique referral code for the user
            string code;
            do
            {
                code = GenerateReferralCode(referralCodeBase);
            } while (await _referralRepository.ReferralCodeExistsAsync(request.ReferrerUserId, code));

            //Build the link
            var link = $"{baseLink}?code={code}";

            //Create referral entity
            var referral = new ReferralEntity
            {
                Id = Guid.NewGuid(),
                ReferrerUserId = request.ReferrerUserId,
                InvitationCode = code,
                LinkGenerated = link,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _referralRepository.CreateReferralAsync(referral);

            return new CreateReferralInviteResponse
            {
                ReferralCode = code,
                LinkGenerated = link
            };
        }

        /// <summary>
        /// Internal function to generate the referral code
        /// </summary>
        /// <param name="referralCodeBase"></param>
        /// <returns>referral code</returns>
        private string GenerateReferralCode(string referralCodeBase)
        {
            var random = new Random();
            return new string(Enumerable.Range(0, 7).Select(_ => referralCodeBase[random.Next(referralCodeBase.Length)]).ToArray());
        }
    }
}
