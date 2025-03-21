using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Dtos.UserDto;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    #region Initialization

    private readonly Context _context;

    public UserRepository(Context context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<uint> CreateUser(UserCreateDto userCreateDto)
    {
        var user = new User
        {
            Uuid = Guid.NewGuid(),
            Username = userCreateDto.Username,
            Email = userCreateDto.Email,
            PhoneNumber = userCreateDto.PhoneNumber,
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            BirthDate = userCreateDto.BirthDate,
            Password = userCreateDto.Password,
            IsShopOwner = false,
            IsAdmin = false,
            IsDeleted = false,
            CreateDate = DateTime.UtcNow
        };

        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<User> GetUserById(uint userId)
    {
        return await _context.User.FindAsync(userId);
    }

    public async Task<User> GetUserByCredentials(string username, string password)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }

    public async Task<User> GetShopOwnerByShopId(uint shopId)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.ShopId == shopId);
    }

    public async Task<List<User>> GetAllAdmins()
    {
        return await _context.User.Where(u => u.IsAdmin == true).ToListAsync();
    }

    public async Task<uint> DeleteUser(uint userId)
    {
        var user = await GetUserById(userId);

        user.IsDeleted = true;

        _context.SaveChanges();
            
        return user.Id;
    }

    #endregion
}
