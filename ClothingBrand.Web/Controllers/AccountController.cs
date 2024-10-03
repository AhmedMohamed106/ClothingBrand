using Application.DTOs.Request.Account;
using Application.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;
        public AccountController(IAccount _account) { 
        this._account = _account;
        }

        [HttpPost("identity/create")]
        public async Task<IActionResult> CreateAccount(CreateAccountDTO newAccount)
        {
            if (!ModelState.IsValid)return BadRequest(ModelState);
            return Ok(await _account.CreateAccountAsync(newAccount));
            
        }
        [HttpPost("identity/login")]
        public async Task<IActionResult> Login(LoginDTO signAcc)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _account.LoginAccountAsync(signAcc);
            if(!response.flag)return BadRequest(response.message);

            return Ok(response.message);

        }
        [HttpPost("identity/refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTockenDto refreshTockenDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _account.RefreshTokenAsync(refreshTockenDto);
            if (!response.flag) return BadRequest(response.message);

            return Ok(response.message);

        }

        [HttpPost("identity/role/create")]
        public async Task<IActionResult> createRole(CraeteRoleDto role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _account.CreateRoleAsync(role);
            if (!response.flag) return BadRequest(response.message);

            return Ok(response.message);

        }

        [HttpGet("identity/role/list")]
        public async Task<IActionResult> GetRoles()
        {
          

            return Ok(await _account.GetRolesAsync());

        }

        [HttpGet("/NewAdmin")]
        public async Task<IActionResult> CreateAdmin()
        {

            await _account.CreateAdmin();
            return Ok();

        }

        [HttpGet("identity/user-with-role")]
        public async Task<IActionResult> GetUserWithRoles()
        {
            

            return Ok(await _account.GetUsersWithRoleAsync());

        }

        [HttpPost("identity/role/change")]
        public async Task<IActionResult> changeRole(ChangeRoleDto role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _account.ChangeUserRoleAsync(role);
            if (!response.flag) return BadRequest(response.message);

            return Ok(response.message);

        }
    }
}
