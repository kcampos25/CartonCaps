namespace CartonCaps.Application.Interfaces.Services
{
    public interface IReferralCodeGenerator
    {
        Task<string> GenerateUniqueReferralCodeAsync(string referralCodeBase);
    }
}
