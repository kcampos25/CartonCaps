using AutoMapper;
using CartonCaps.Application.DTOs;
using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Services;
using CartonCaps.Application.Interfaces.Validators;
using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Services
{
    /// <summary>
    /// Service responsible for managing visits or clicks that are executed on friend invitation links
    /// </summary>
    public class ReferralVisitService : IReferralVisitService
    {
        private readonly IReferralVisitRepository _referralVisitRepository;
        private readonly IReferralServiceValidator _referralServiceValidator;
        private readonly IMapper _mapper;
        private readonly TimeSpan _clickWindow = TimeSpan.FromMinutes(1);

        public ReferralVisitService(IReferralVisitRepository referralVisitRepository, IMapper mapper, IReferralServiceValidator referralServiceValidator)
        {
            _referralVisitRepository = referralVisitRepository;
            _referralServiceValidator = referralServiceValidator;
            _mapper = mapper;
        }

        /// <summary>
        /// Record each visit to the link, that is, each time the
        /// friend to whom the link is sent clicks on it.
        /// </summary>
        /// <param name="referralRedirectRequest"></param>
        /// <returns>Response objects from visited links</returns>
        public async Task<ReferralVisitResponse> CreateReferralVisit(ReferralRedirectRequest referralRedirectRequest)
        {
            //Check if the referral code exists; if not, return an exception.
            var referral = await _referralServiceValidator.GetReferralOrThrowAsync(referralRedirectRequest.ReferralCode);

            if (referral.Status == "Completed")
                return BuildReferralVisitResponse(false, "Referral is already completed. No further clicks are tracked.", referralRedirectRequest.RedirectUrl);

            if (await IsClickBlockedAsync(referral.Id, referralRedirectRequest.IpAddress))
                return BuildReferralVisitResponse(false, $"Click ignored: IP {referralRedirectRequest.IpAddress} already clicked in the last {_clickWindow.TotalMinutes} minutes.", referralRedirectRequest.RedirectUrl);

            var visit = new ReferralVisitEntity
            {
                Id = Guid.NewGuid(),
                IpAddress = referralRedirectRequest.IpAddress,
                VisitedAt = DateTime.UtcNow,
                ReferralId = referral.Id
            };
            await _referralVisitRepository.CreateReferralVisit(visit);

            return BuildReferralVisitResponse(true, "", referralRedirectRequest.RedirectUrl);
        }

        /// <summary>
        /// Function responsible for listing all records of visits or clicks on links.
        /// </summary>
        /// <returns>list of registered visits</returns>
        public async Task<IEnumerable<ReferralVisitHistoryResponse>> GetReferralVisitHistoryAsync()
        {
            var referralVisits = await _referralVisitRepository.GetReferralVisitHistoryAsync();
            return _mapper.Map<IEnumerable<ReferralVisitHistoryResponse>>(referralVisits);
        }

        /// <summary>
        /// Validates the last click record from a specific referral and IP address
        /// within a set time window to determine if the link click record is blocked.
        /// </summary>
        /// <param name="referralId"></param>
        /// <param name="ipAddress"></param>
        /// <returns>List of a user's referrals(name,status)</returns>
        private async Task<bool> IsClickBlockedAsync(Guid referralId, string ipAddress)
        {
            var lastClick = await _referralVisitRepository
                .GetLastClickByIpAsync(referralId, ipAddress, _clickWindow);

            return lastClick != null;
        }

        /// <summary>
        /// Internal function that builds the response when clicks
        /// on links are recorded
        /// </summary>
        /// <param name="registered"></param>
        /// <param name="message"></param>
        /// <param name="redirectUrl"></param>
        /// <returns>Response objects from visited links</returns>
        private ReferralVisitResponse BuildReferralVisitResponse(bool registered, string message, string redirectUrl)
        {
            return new ReferralVisitResponse
            {
                Registered = registered,
                Message = message,
                RedirectUrl = redirectUrl
            };
        }
    }
}
