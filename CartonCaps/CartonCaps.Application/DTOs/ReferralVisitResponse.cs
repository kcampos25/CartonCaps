namespace CartonCaps.Application.DTOs
{
    public class ReferralVisitResponse
    {
        //Flag to identify if the click was registered
        public bool Registered { get; set; }

        //Message to store the response from the link click log
        public string? Message { get; set; }

        //Redirect to the store to download the app
        public string RedirectUrl { get; set; } = null!;
    }
}
