using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using static Infrastructure.Dtos.UserDto;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    #region Initialization

    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    #endregion

    // retrieving userId from token
    // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    #region Apis

    [HttpPost(nameof(CreateTemporaryUserAndSendRegistrationOtp))]
    public async Task<IActionResult> CreateTemporaryUserAndSendRegistrationOtp([FromBody] string phoneNumber)
    {
        if (!Regex.IsMatch(phoneNumber, "^9\\d{9}$"))
        {
            return BadRequest("Invalid phone number.");
        }

        var userId = await _userRepository.CreateTemporaryUser(phoneNumber);

        var otp = await _userRepository.SetLastOtp(userId);

        return Ok(otp);
    }

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(UserRegisterDto userCreateDto)
    {
        if (userCreateDto.Password != userCreateDto.ConfirmPassword)
        {
            return BadRequest("Password and confirm password do not match.");
        }

        var user = await _userRepository.RegisterUser(userCreateDto);

        var token = GenerateJwtToken(user);

        return Ok(token);
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(UserCredentialsDto userGetByCredentialsDto)
    {
        if (!Regex.IsMatch(userGetByCredentialsDto.PhoneNumber, "^9\\d{9}$"))
        {
            return BadRequest("Invalid phone number.");
        }

        var user = await _userRepository.GetUserByCredentials(userGetByCredentialsDto);

        if (user is null)
        {
            return BadRequest("Phone number or password is incorrect.");
        }

        var token = GenerateJwtToken(user);

        return Ok(token);
    }

    [HttpPost(nameof(SetNewPasswordForUser))]
    public async Task<IActionResult> SetNewPasswordForUser(UserSetNewPasswordForUserDto userSetNewPasswordDto)
    {
        if (!Regex.IsMatch(userSetNewPasswordDto.PhoneNumber, "^9\\d{9}$"))
        {
            return BadRequest("Invalid phone number.");
        }

        if (userSetNewPasswordDto.Password != userSetNewPasswordDto.ConfirmPassword)
        {
            return BadRequest("Password and confirm password do not match.");
        }

        var userCredentialsDto = new UserCredentialsDto
        {
            PhoneNumber = userSetNewPasswordDto.PhoneNumber,
            Password = userSetNewPasswordDto.Password
        };

        var userId = await _userRepository.SetNewPasswordForUser(userCredentialsDto);

        return Ok(userId);
    }

    [HttpGet($"{nameof(GetUserById)}/{{userId}}")]
    public async Task<IActionResult> GetUserById(uint userId)
    {
        var user = await _userRepository.GetUserById(userId);

        return Ok(user);
    }

    [HttpGet($"{nameof(GetShopOwnerByShopId)}/{{shopId}}")]
    public async Task<IActionResult> GetShopOwnerByShopId(uint shopId)
    {
        var shopOwner = await _userRepository.GetShopOwnerByShopId(shopId);

        return Ok(shopOwner);
    }

    [HttpGet(nameof(GetAllAdmins))]
    public async Task<IActionResult> GetAllAdmins()
    {
        var admins = await _userRepository.GetAllAdmins();

        return Ok(admins);
    }

    [HttpDelete($"{nameof(DeleteUser)}/{{userId}}")]
    public async Task<IActionResult> DeleteUser(uint userId)
    {
        await _userRepository.DeleteUser(userId);

        return Ok(userId);
    }

    #endregion

    #region Utilities

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    #endregion
}
