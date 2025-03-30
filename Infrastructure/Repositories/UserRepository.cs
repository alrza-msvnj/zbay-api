using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
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
        if (string.IsNullOrWhiteSpace(userCreateDto.FirstName))
        {
            throw new FormatException("Please provide a first name.");
        }

        var existingUser = await GetUserByPhoneNumber(userCreateDto.PhoneNumber);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("User already exists.");
        }

        var user = new User
        {
            Uuid = Guid.NewGuid(),
            PhoneNumber = userCreateDto.PhoneNumber,
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
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

    public async Task<User> GetUserByPhoneNumber(string phoneNumber)
    {
        if (Regex.IsMatch(phoneNumber, "^09\\d{9}$"))
        {
            throw new FormatException("Invalid phone number.");
        }

        return await _context.User.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<User> GetUserByCredentials(UserGetByCredentialsDto userGetByCredentialsDto)
    {
        if (!Regex.IsMatch(userGetByCredentialsDto.PhoneNumber, "^09\\d{9}$"))
        {
            throw new FormatException("Invalid phone number.");
        }

        return await _context.User.FirstOrDefaultAsync(u => u.IsDeleted == false && u.PhoneNumber == userGetByCredentialsDto.PhoneNumber && u.Password == userGetByCredentialsDto.Password);
    }

    public async Task<User> GetShopOwnerByShopId(uint shopId)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.IsDeleted == false && u.ShopId == shopId);
    }

    public async Task<List<User>> GetAllAdmins()
    {
        return await _context.User.Where(u => u.IsDeleted == false && u.IsAdmin == true).ToListAsync();
    }

    public async Task<uint> DeleteUser(uint userId)
    {
        var user = await GetUserById(userId);

        if (user is null)
        {
            throw new InvalidOperationException("User does not exist.");
        }

        user.IsDeleted = true;

        _context.SaveChanges();
            
        return user.Id;
    }

    #endregion
}
