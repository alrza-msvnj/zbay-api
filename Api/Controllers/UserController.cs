using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static Infrastructure.Dtos.UserDto;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    #region Initialization

    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    #endregion

    #region Apis

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(UserCreateDto userCreateDto)
    {
        if (userCreateDto.Password != userCreateDto.ConfirmPassword)
        {
            return BadRequest("Password and confirm password do not match.");
        }

        var userId = await _userRepository.CreateUser(userCreateDto);

        return Ok(userId);
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(UserGetByCredentialsDto userGetByCredentialsDto)
    {
        var user = await _userRepository.GetUserByCredentials(userGetByCredentialsDto);

        // login user

        return Ok();
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
}
