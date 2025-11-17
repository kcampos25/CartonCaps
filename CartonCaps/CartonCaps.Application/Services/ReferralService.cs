using AutoMapper;
using CartonCaps.Application.DTOs;
using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Services;
using CartonCaps.Application.Interfaces.Validators;
using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Services
{
    /// <summary>
    /// Service responsible for managing referred users
    /// </summary>
    public class ReferralService: IReferralService
    {
        private readonly IReferralRepository _referralRepository;
        private readonly IUserServiceValidator _userServiceValidator;
        private readonly IReferralCodeGenerator _referralCodeGenerator;
        private readonly IMapper _mapper;

        public ReferralService(IReferralRepository referralRepository, IMapper mapper, IUserServiceValidator userServiceValidator, IReferralCodeGenerator referralCodeGenerator)
        {
            _referralRepository = referralRepository;
            _referralCodeGenerator = referralCodeGenerator;
            _userServiceValidator = userServiceValidator;
            _mapper = mapper;
        }

        /// <summary>
        /// Function responsible for obtaining a list of users who have been referred by a user(my referrals).
        /// </summary>
        /// <param name="referrerUserId"></param>
        /// <returns>List of a user's referrals(name,status)</returns>
        public async Task<IEnumerable<UserReferralResponse>> GetReferralByUserAsync(Guid referrerUserId)
        {
            await _userServiceValidator.ValidateUserExistsOrThrowAsync(referrerUserId);

            var invites = await _referralRepository.GetReferralByUserAsync(referrerUserId);
            return _mapper.Map<IEnumerable<UserReferralResponse>>(invites);
        }

        /// <summary>
        /// This function performs the following:
        ///     1- Generates a unique referral code.
        ///     2- Builds the invitation link.
        ///     3- Saves the invitation record in the referrals table (Pending status).
        /// </summary>
        /// <param name="referralInviteRequest"></param>
        /// <param name="baseLink"></param>
        /// <param name="referralCodeBase"></param>
        /// <returns>Object containing the invitation link and referral code</returns>
        public async Task<CreateReferralInviteResponse> CreateReferralAsync(CreateReferralInviteRequest referralInviteRequest, string baseLink, string referralCodeBase)
        {
            if (referralInviteRequest == null)
                throw new ArgumentNullException(nameof(referralInviteRequest));

            if (string.IsNullOrWhiteSpace(baseLink))
                throw new ArgumentException("Base link is required.");

            if (!Uri.TryCreate(baseLink, UriKind.Absolute, out _))
                throw new ArgumentException("Invalid base link URL.");

            if (string.IsNullOrWhiteSpace(referralCodeBase))
                throw new ArgumentException("Referral code base is required.");

            //Checks if the user exists; if not, it returns an exception.
            await _userServiceValidator.ValidateUserExistsOrThrowAsync(referralInviteRequest.ReferrerUserId);

            //Generate unique referral code
            var referralCode = await _referralCodeGenerator.GenerateUniqueReferralCodeAsync(referralCodeBase);

            //Build the link
            var link = $"{baseLink}?code={referralCode}";

            //Create referral entity
            var referral = new ReferralEntity
            {
                Id = Guid.NewGuid(),
                ReferrerUserId = referralInviteRequest.ReferrerUserId,
                ReferralCode = referralCode,
                LinkGenerated = link,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            await _referralRepository.CreateReferralAsync(referral);

            return new CreateReferralInviteResponse
            {
                ReferralCode = referralCode,
                LinkGenerated = link,
                Message = "Referral created successfully."
            };
        }
    }
}
