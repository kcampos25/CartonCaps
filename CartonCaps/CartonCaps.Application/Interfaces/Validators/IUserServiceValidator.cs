using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Interfaces.Validators
{
    public interface IUserServiceValidator
    {
        Task<UserEntity> ValidateUserExistsOrThrowAsync(Guid userId);
    }
}
