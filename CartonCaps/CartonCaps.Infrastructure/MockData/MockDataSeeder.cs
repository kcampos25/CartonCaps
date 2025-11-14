using CartonCaps.Infrastructure.Data;

namespace CartonCaps.Infrastructure.MockData
{
    /// <summary>
    /// Class for loading test data 
    /// </summary>
    public static class MockDataSeeder
    {
        public static void Seed(CartonCapsContext context)
        {
            //Test data for users

            var kennethId = Guid.Parse("5fbf7d9b-2d26-49cf-8941-d94863c5104a");
            var keylinId = Guid.Parse("fa90e2dc-a7be-4722-a64d-e02d3f4eb6f2");
            var gabrielId = Guid.Parse("e3afb89a-b04c-4570-9858-f94eb794cdfc");
            var stevenId = Guid.Parse("4077da55-467b-47b8-a73b-257047c35606");

            var users = new List<User>
            {
                new User
                {
                    Id=kennethId,
                    FirstName="Kenneth",
                    LastName="Campos",
                    Birthday=new DateTime(1991, 6, 25),
                    ReferralCode="XY7G4D"
                },

                new User
                {
                    Id=keylinId,
                    FirstName="Keylin",
                    LastName="Rodriguez",
                    Birthday=new DateTime(1991, 10, 21),
                    ReferralCode="XZ7TE4S",
                    ReferredById=kennethId
                },

                 new User
                {
                    Id=gabrielId,
                    FirstName="Gabriel",
                    LastName="Mora",
                    Birthday=new DateTime(1989, 01, 10),
                    ReferralCode="WZ7HE3S"
                },

                 new User
                {
                    Id=stevenId,
                    FirstName="Steven",
                    LastName="Murillo",
                    Birthday=new DateTime(1989, 02, 11),
                    ReferralCode="QT2HE3S",
                    ReferredById=kennethId
                },
            };

            context.AddRange(users);

            //Test data for referrals
            var referrals = new List<Referral>
            {
                new Referral
                {
                    Id=Guid.Parse("243b5d59-6a81-40f6-ab90-8bb7be9b52e6"),
                    ReferrerUserId=kennethId,
                    LinkGenerated="https://cartoncaps.link/abfilefa90p?referral_code=XY7G4D-01",
                    Status="Completed",
                    InviteName = "Keylin Rodriguez",
                    InvitationCode = "XY7G4D-01"
                },

                 new Referral
                {
                    Id=Guid.Parse("d3bad86a-cf93-4983-bfd4-30534a93fa54"),
                    ReferrerUserId=kennethId,
                    LinkGenerated="https://cartoncaps.link/abcfr60p?referral_code=XY7G4D-02",
                    Status="Completed",
                    InviteName = "Steven Murillo",
                    InvitationCode = "XY7G4D-02"
                },

                   new Referral
                {
                    Id=Guid.Parse("05dad853-9969-40d6-885a-003e10376458"),
                    ReferrerUserId=gabrielId,
                    LinkGenerated="https://cartoncaps.link/abdfileca80p?referral_code=WZ7HE3S",
                    Status="Pending",
                    InvitationCode = "WZ7HE3S-01"
                },
            };

            context.AddRange(referrals);

            //Test data for visits
            var visits = new List<ReferralVisit>
            {
                new ReferralVisit
                {
                    Id=Guid.Parse("4ef0d009-255a-4a53-8201-855bd48c7511"),
                    ReferralId=referrals[0].Id,
                    VisitedAt=DateTime.UtcNow.AddHours(-3),
                    IpAddress="190.123.45.67",

                },

              new ReferralVisit
                {
                    Id=Guid.Parse("7a5cb391-3843-425c-bda7-a248ecd12cf5"),
                    ReferralId=referrals[1].Id,
                    VisitedAt=DateTime.UtcNow.AddHours(-1),
                    IpAddress="190.123.44.90",
                },

               new ReferralVisit
                {
                    Id=Guid.Parse("0f89cf5c-b496-41d1-908d-3a69a948c3a9"),
                    ReferralId=referrals[2].Id,
                    VisitedAt=DateTime.UtcNow.AddHours(-2),
                    IpAddress="200.56.78.90",
                },

            };

            context.AddRange(visits);
            context.SaveChanges();
        }
    }
}
