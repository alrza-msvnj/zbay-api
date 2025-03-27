using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost(nameof(CreateUser))]
    public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
    {
        var userId = await _userRepository.CreateUser(userCreateDto);

        return Ok(userId);
    }

    [HttpGet($"{nameof(GetUserById)}/{{userId}}")]
    public async Task<IActionResult> GetUserById(uint userId)
    {
        var user = await _userRepository.GetUserById(userId);

        return Ok(user);
    }

    [HttpGet($"{nameof(GetUserByCredentials)}/{{username}}/{{password}}")]
    public async Task<IActionResult> GetUserByCredentials(string username, string password)
    {
        var user = await _userRepository.GetUserByCredentials(username, password);

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
