using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using ClothingBrand.Application.Common.DTO.Request.Account;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfaces
{
    public interface IAccount
    {
        Task CreateAdmin();
        Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model);
        Task<LoginResponse> LoginAccountAsync(LoginDTO model);

        Task<GeneralResponse> CreateRoleAsync(CraeteRoleDto model);
        Task<IEnumerable<GetRoleDTO>> GetRolesAsync();

        Task<IEnumerable<GetUserWithRolesDTo>> GetUsersWithRoleAsync();
        Task<GeneralResponse> ChangeUserRoleAsync(ChangeRoleDto model);
        Task<LoginResponse> RefreshTokenAsync(string Retoken);
        Task<GeneralResponse> ConfirmEmail(string userID, string Token);
        Task<GeneralResponse> RemoveUser(string id);
        Task<GeneralResponse> LogOut(string userId);
        Task SendEmail(string userId);
        Task<GeneralResponse> ResetPassword(string userId, string token, string password);
         Task ForgetPassword(string userEmail,string origion);
        Task<GeneralResponse> ChangePassword(ChangePasswordDTO changePasswordDTO);
        Task<GeneralResponse> emailExists(string email);
        Task<bool> UserExistsAsync(string userId);
        Task<string> GetRoleOfUser(string userId);

    }
}
