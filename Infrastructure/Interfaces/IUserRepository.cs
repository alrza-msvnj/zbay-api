using Domain.Entities;
using static Infrastructure.Dtos.UserDto;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<long> CreateTemporaryUser(string phoneNumber);
    Task<int> SetLastOtp(long userId);
    Task<User> RegisterUser(UserRegisterDto userRegisterDto);
    Task<long> SetNewPasswordForUser(UserCredentialsDto userCredentialsDto);
    Task<User> GetUserById(long userId);
    Task<User> GetUserByPhoneNumber(string phoneNumber);
    Task<User> GetUserByCredentials(UserCredentialsDto userGetByCredentialsDto);
    Task<User> GetShopOwnerByShopId(long shopId);
    Task<List<User>> GetAllAdmins();
    Task<long> DeleteUser(long userId);
}
