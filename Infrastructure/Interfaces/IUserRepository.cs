using Domain.Entities;
using static Infrastructure.Dtos.UserDto;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<uint> CreateUser(UserCreateDto userCreateDto);
    Task<User> GetUserById(uint userId);
    Task<User> GetUserByCredentials(string username, string password);
    Task<User> GetShopOwnerByShopId(uint shopId);
    Task<List<User>> GetAllAdmins();
    Task<uint> DeleteUser(uint userId);
}
