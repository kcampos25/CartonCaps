using CartonCaps.Domain.Entities;


namespace CartonCaps.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetUserById(Guid referrerUserId);
    }
}
