
namespace CartonCaps.Application.DTOs
{
    public class ReferralRedirectRequest
    {
        //Unique referral code sent to friend
        public string ReferralCode { get; set; } = null!;

        //IP address from where the link was clicked
        public string IpAddress { get; set; } = null!;

        //Redirect to the store to download the app
        public string RedirectUrl { get; set; } = null!;
    }
}
