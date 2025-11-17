using CartonCaps.Application.DTOs;
using CartonCaps.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Api.Controllers
{
    /// <summary>
    /// API that exposes the endpoints to manage visits or clicks that are executed on friend invitation links
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReferralsVisitController : ControllerBase
    {
        private readonly IReferralVisitService _referralVisitService;
        private readonly string _redirectURL;
        private readonly ILogger<ReferralsVisitController> _logger;

        public ReferralsVisitController(IReferralVisitService referralVisitService, IConfiguration configuration, ILogger<ReferralsVisitController> logger)
        {
            _referralVisitService = referralVisitService;
            _redirectURL = configuration["Referral:RedirectTo"];
            _logger = logger;
        }

        /// <summary>
        /// Endpoint responsible for registering visits or clicks on the link to mitigate abuse.
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns>Redirects to download the app from the store</returns>
        [HttpPost("redirect")]
        public async Task<IActionResult> CreateReferralVisit([FromQuery] string referralCode)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";

            var referralRedirectRequest = new ReferralRedirectRequest
            {
                ReferralCode = referralCode,
                IpAddress = ipAddress,
                RedirectUrl = _redirectURL
            };

            var result = await _referralVisitService.CreateReferralVisit(referralRedirectRequest);

            //This section is solely for challenge purposes to review certain messages from the service: CreateReferralVisit.
            _logger.LogInformation(
                  "Referral click from IP {Ip} for code {Code}: {Message}",
                  referralRedirectRequest.IpAddress,
                  referralRedirectRequest.ReferralCode,
                  result.Message
              );
            return Redirect(result.RedirectUrl);
        }

        /// <summary>
        /// Important: This endpoint is not part of the challenge or the requirements.
        /// Objective: A test help endpoint that lists historical information about
        /// clicks recorded on links by invited friends.
        /// </summary>
        /// <returns>list of registered visits</returns>
        [HttpGet("referralVisitHistory")]
        public async Task<IActionResult> GetReferralVisitHistory()
        {
            var visits = await _referralVisitService.GetReferralVisitHistoryAsync();
            return visits == null ? NotFound() : Ok(visits);
        }
    }
}
