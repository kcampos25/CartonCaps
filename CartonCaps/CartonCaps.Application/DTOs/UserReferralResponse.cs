namespace CartonCaps.Application.DTOs
{
    public class UserReferralResponse
    {
        //Name and last name of the invited person
        public string? InviteeName { get; set; } = null!;

        //Referral status
        public string Status { get; set; } = null!;
    }
}
