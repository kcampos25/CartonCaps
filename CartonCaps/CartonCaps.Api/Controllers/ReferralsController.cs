using CartonCaps.Application.DTOs;
using CartonCaps.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Api.Controllers
{
    /// <summary>
    /// API that exposes the endpoints related to the referred users
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralService _referralService;
        private readonly string _baseReferralLink;
        private readonly string _baseReferralCode;

        public ReferralsController(IReferralService referralService, IConfiguration configuration)
        {
            _referralService = referralService;
            _baseReferralLink = configuration["Referral:BaseLink"];
            _baseReferralCode = configuration["Referral:BaseReferralCode"];
        }

        /// <summary>
        /// Endpoint responsible for obtaining a list of users who have been referred by a user(my referrals).
        /// </summary>
        /// <param name="referrerUserId"></param>
        /// <returns>List of a user's referrals(name,status)</returns>
        [HttpGet("{referrerUserId}/referrals")]
        public async Task<IActionResult> GetReferralByUser(Guid referrerUserId)
        {
            var result = await _referralService.GetReferralByUserAsync(referrerUserId);

            if (result == null || !result.Any())
                return NotFound(new { message = "No referrals found for this user."});

            return Ok(result);
        }

        /// <summary>
        /// Endpoint responsible for generating the invitation link.
        /// It generates the link, saves the referral record, and returns the link.
        /// </summary>
        /// <param name="referralInviteRequest"></param>
        /// <returns>Object containing the invitation link and referral code</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateReferral([FromBody] CreateReferralInviteRequest referralInviteRequest)
        {
            var result = await _referralService.CreateReferralAsync(referralInviteRequest, _baseReferralLink, _baseReferralCode);
            return Ok(result);
        }
    }
}
