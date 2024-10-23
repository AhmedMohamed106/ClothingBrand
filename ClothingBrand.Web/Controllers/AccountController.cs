using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.interfaces;
using ClothingBrand.Application.Common.DTO.Request.Account;
using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

            SetRefreshTokenInCookie(response.RefreshToken);
             
            return Ok(response);

        }
        [HttpPost("identity/refresh-token")]
        public async Task<IActionResult> RefreshToken()//RefreshTockenDto refreshTockenDto
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _account.RefreshTokenAsync(refreshToken);
            if (!response.flag) return BadRequest(response);
            //if()
            SetRefreshTokenInCookie(response.RefreshToken);

            return Ok(response);

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

        [HttpGet("identity/LogOut")]
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



        [HttpPost("identity/ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            {
                var result = await _account.ResetPassword(resetPassword.userId, resetPassword.Token, resetPassword.password);
                if (result.flag)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }




        }



        [HttpGet("identity/ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([Required] string email,string origin)
        {
            await _account.ForgetPassword(email, origin);
            return Ok();
        }
        [HttpPost("identity/ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {

            if (ModelState.IsValid)
            {
                var res = await _account.ChangePassword(changePasswordDTO);

                return Ok(res);
            }
            return BadRequest(ModelState);



        }

        [HttpGet("EmailExists")]
        public async Task<IActionResult> EmailExists(string email)
        {
            var res= _account.emailExists(email);
            return Ok(res);
        }
        [HttpGet("userExists")]
        public async Task<IActionResult> UserExists(string id)
        {
            var res = _account.UserExistsAsync(id);
            return Ok(res);
        }

         [HttpGet("identity/CurrentUserName")]
        public async Task<IActionResult> CurrentUserName()
        {
            var res = User.FindFirstValue("FullName");   
            if(res == null)return Ok(new GeneralResponse(false,"no Login" ));
            return Ok(new GeneralResponse(true,res));
        }

        [HttpGet("identity/CurrentUserRole")]
        public async Task<IActionResult> CurrentUserRole(string userId)
        {
            var res=  await _account.GetRoleOfUser(userId);
            if (res == null) return Ok( "no  accesss");
            var result=new GeneralResponse();
            if (res== "Admin")  result = new GeneralResponse(true, res);
            else
              result=new GeneralResponse(false,res);
            return Ok(result);
        }


        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(7),//.ToLocalTime()
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }


    }


}


//ahmedshapan183@gmail.com
//Shaban@123
//    Ahmed@123