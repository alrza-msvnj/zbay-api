using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Data;
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

    public async Task<long> CreateTemporaryUser(string phoneNumber)
    {
        if (!Regex.IsMatch(phoneNumber, "^9\\d{9}$"))
        {
            throw new FormatException("Invalid phone number.");
        }

        var existingUser = await GetUserByPhoneNumber(phoneNumber);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("User already exists.");
        }

        var user = new User
        {
            Uuid = Guid.NewGuid(),
            PhoneNumber = phoneNumber,
            Role = UserRole.Temporary,
            IsDeleted = false,
            CreateDate = DateTime.UtcNow
        };

        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<User> RegisterUser(UserRegisterDto userRegisterDto)
    {
        var user = await GetUserByPhoneNumber(userRegisterDto.PhoneNumber);

        if (user is null)
        {
            throw new InvalidOperationException("User does not exist.");
        }

        user.FirstName = userRegisterDto.FirstName;
        user.LastName = userRegisterDto.LastName;
        user.Password = userRegisterDto.Password;
        user.Role = UserRole.Buyer;

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<long> SetNewPasswordForUser(UserCredentialsDto userCredentialsDto)
    {
        var user = await GetUserByPhoneNumber(userCredentialsDto.PhoneNumber);

        user.Password = userCredentialsDto.Password;

        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<User> GetUserById(long userId)
    {
        return await _context.User.FindAsync(userId);
    }

    public async Task<User> GetUserByPhoneNumber(string phoneNumber)
    {
        if (!Regex.IsMatch(phoneNumber, "^9\\d{9}$"))
        {
            throw new FormatException("Invalid phone number.");
        }

        return await _context.User.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<User> GetUserByCredentials(UserCredentialsDto userGetByCredentialsDto)
    {
        if (!Regex.IsMatch(userGetByCredentialsDto.PhoneNumber, "^9\\d{9}$"))
        {
            throw new FormatException("Invalid phone number.");
        }

        return await _context.User.FirstOrDefaultAsync(u => !u.IsDeleted && u.PhoneNumber == userGetByCredentialsDto.PhoneNumber && u.Password == userGetByCredentialsDto.Password);
    }

    public async Task<User> GetShopOwnerByShopId(long shopId)
    {
        return await _context.User.FirstOrDefaultAsync(u => !u.IsDeleted && u.ShopId == shopId);
    }

    public async Task<List<User>> GetAllAdmins()
    {
        return await _context.User.Where(u => !u.IsDeleted && u.Role == UserRole.Admin).ToListAsync();
    }

    public async Task<long> DeleteUser(long userId)
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

    public async Task<int> SetLastOtp(long userId)
    {
        var user = await GetUserById(userId);

        if (user is null)
        {
            throw new InvalidOperationException("User does not exist");
        }

        var code = new Random().Next(1000, 9999);

        user.LastOtp = code;

        await _context.SaveChangesAsync();

        return code;
    }

    #endregion
}
