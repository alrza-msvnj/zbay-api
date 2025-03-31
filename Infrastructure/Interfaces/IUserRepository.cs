using Domain.Entities;
using static Infrastructure.Dtos.UserDto;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<uint> CreateUser(UserCreateDto userCreateDto);
    Task<uint> SetNewPasswordForUser(UserCredentialsDto userCredentialsDto);
    Task<User> GetUserById(uint userId);
    Task<User> GetUserByPhoneNumber(string phoneNumber);
    Task<User> GetUserByCredentials(UserCredentialsDto userGetByCredentialsDto);
    Task<User> GetShopOwnerByShopId(uint shopId);
    Task<List<User>> GetAllAdmins();
    Task<uint> DeleteUser(uint userId);
}
