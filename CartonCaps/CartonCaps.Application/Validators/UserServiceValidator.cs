using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Validators;
using CartonCaps.Domain.Entities;

namespace CartonCaps.Application.Validators
{
    public class UserServiceValidator: IUserServiceValidator
    {
        private readonly IUserRepository _userRepository;

        public UserServiceValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Function responsible for validating that a user exists and is a valid user.
        /// </summary>
        /// <param name="referrerUserId"></param>
        /// <returns>UserEntity object or an exception</returns>
        public async Task<UserEntity> ValidateUserExistsOrThrowAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.");

            var user = await _userRepository.GetUserById(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return user;
        }
    }
}
