using Application.DTOs.Request.Account;
using Application.interfaces;
using ClothingBrand.Application.Common.DTO.Request.Account;
using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;
        public AccountController(IAccount _account)
        {
            this._account = _account;
        }

        [HttpPost("identity/create")]
        public async Task<IActionResult> CreateAccount(CreateAccountDTO newAccount)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _account.CreateAccountAsync(newAccount));

        }
        [HttpPost("identity/login")]
        public async Task<IActionResult> Login(LoginDTO signAcc)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _account.LoginAccountAsync(signAcc);
            if (!response.flag) return BadRequest(response.message);

            return Ok(response);

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
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            await _account.ConfirmEmail(userId, token);
            return Ok();
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var res = await _account.RemoveUser(id);
            if (res.flag)
            {
                return Ok(res.message);
            }
            return BadRequest(res.message);
        }

        [HttpGet("LogOut")]
        public async Task<IActionResult> Logout()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _account.LogOut(id);
            return Ok();
        }
        [HttpGet("sendEmail")]
        public async Task<IActionResult> sentEmailConfirm(string id)
        {
            await _account.SendEmail(id);
            return Ok();
        }



        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string userId, string token, string password)
        {
            {
                var result = await _account.ResetPassword(userId, token, password);
                if (result.flag)
                {
                    return Ok(result.message);
                }
                return BadRequest(result.message);
            }




        }



        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([Required] string email)
        {
            await _account.ForgetPassword(email);
            return Ok();
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {

            if (ModelState.IsValid)
            {
                var res = await _account.ChangePassword(changePasswordDTO);

                return Ok(res.message);
            }
            return BadRequest(ModelState);



        }
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> CurrentUserId()
        {
            var res = User.FindFirstValue("FullName");
            if (res == null) return Ok(false);
            return Ok(res);
        }


    }


}
