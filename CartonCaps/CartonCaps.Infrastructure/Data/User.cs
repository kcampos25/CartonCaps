namespace CartonCaps.Infrastructure.Data
{
    /// <summary>
    /// Model that simulates the App's users table.
    /// </summary>
    public class User
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

        //Relation to the referring user
        public Guid? ReferredById { get; set; }

        //Property to directly access the User object that referred it.
        public User? ReferredBy { get; set; }
    }
}
