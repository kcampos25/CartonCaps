namespace CartonCaps.Domain.Entities
{
    public class UserEntity
    {
        //Unique identifier.
        public Guid Id { get; set; }

        //User name.
        public string FirstName { get; set; } = null!;

        //Last user name.
        public string LastName { get; set; } = null!;

        //User's Birthday
        public DateTime? Birthday { get; set; }

        //record creation date
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Unique code per user.
        public string ReferralCode { get; set; } = null!;
    }
}
