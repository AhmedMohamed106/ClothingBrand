

using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.interfaces;
using ClothingBrand.Domain.Models;
using ClothingBrand.Infrastructure.DataContext;
using ClothingBrand.Domain.Models;
using ClothingBrand.Infrastructure.DataContext;
using Mapster;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClothingBrand.Infrastructure.Emails;

using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using ClothingBrand.Application.Contract;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using ClothingBrand.Application.Common.DTO.Request.Account;
using Microsoft.AspNetCore.Mvc;
using ClothingBrand.Infrastructure.Migrations;

namespace infrastructure.Repos
{
    public class AccountRepository : IAccount
    {
        private RoleManager<IdentityRole> roleManager;

        private UserManager<ApplicationUser> userManager;
        private IConfiguration config;
        private ApplicationDbContext _context;
        private SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailService _emailService;

        public AccountRepository(RoleManager<IdentityRole> roleManager,
          UserManager<ApplicationUser> userManager, IConfiguration config, ApplicationDbContext context,
          SignInManager<ApplicationUser> signInManager, IHttpContextAccessor contextAccessor, IEmailService emailService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.config = config;
            this.signInManager = signInManager;
            this._context = context;
            _contextAccessor = contextAccessor;
            _emailService = emailService;
        }
        private async Task<ApplicationUser> FindUserByEmailAsync(string email) => await userManager.FindByEmailAsync(email);
        private async Task<IdentityRole> FindRoleByNameAsync(string user) => await roleManager.FindByNameAsync(user);

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var userClaims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),

                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, (await userManager.GetRolesAsync(user)).FirstOrDefault().ToString()),
                     new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim("FullName", user.Name)
                };





                var token = new JwtSecurityToken(
                     issuer: config["Jwt:ValidIssuer"],
                     audience: config["Jwt:ValidAudiance"],
                     claims: userClaims,
                     expires: DateTime.Now.ToLocalTime().AddMinutes(1),
                     signingCredentials: credentials
                      );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return null!;

            }
        }




        private async Task<GeneralResponse> AssignUserToRoleAsync(ApplicationUser user, IdentityRole role)
        {
            if (user == null || role == null) return new GeneralResponse(false, "Model state can't be Empty");
            if (await FindRoleByNameAsync(role.Name) == null) await CreateRoleAsync(role.Adapt(new CraeteRoleDto()));
            IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
                return new GeneralResponse(true, $"{user.Name} is assigne to {role.Name} role");
            else
                return new GeneralResponse(false, result.Errors.ToString());

        }



        public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeRoleDto model)
        {
            var user = await FindUserByEmailAsync(model.UserEmail);
            var role = await FindRoleByNameAsync(model.RoleName);
            if (user == null) return new GeneralResponse(false, "user not Found");
            if (role == null) return new GeneralResponse(false, "role not Found");
            var PreviousRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var RemoveOldRole = await userManager.RemoveFromRoleAsync(user, PreviousRole);
            if (!RemoveOldRole.Succeeded) return new GeneralResponse(false, RemoveOldRole.Errors.ToString());
            var newRole = await userManager.AddToRoleAsync(user, role.Name);
            if (!newRole.Succeeded) return new GeneralResponse(false, RemoveOldRole.Errors.ToString());
            return new GeneralResponse(true, "Role Changed");

        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                if (await FindUserByEmailAsync(model.Email) != null) return new GeneralResponse(false, "User is already Created ");
                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    UserName = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    string error = "";
                    foreach (var err in result.Errors)
                    {
                        error += err.Description;
                    }

                    return new GeneralResponse(false, error);

                }
                await SendConfirmationEmail(user);
                
              return  await AssignUserToRoleAsync(user,new IdentityRole() { Name=model.Role });


            }
            catch (Exception ex)
            {
                return new GeneralResponse(false, ex.Message);
            }
        }

        public async Task CreateAdmin()
        {
            try
            {
                if (await FindRoleByNameAsync("Admin") != null) return;
                var admin = new CreateAccountDTO()
                {
                    Email = "ahmedshapan183@gmail.com",
                    Password = "Ahmed@123",
                    Name = "ahmed Shaban",
                    Role = "Admin"
                };
                await CreateAccountAsync(admin);
            }
            catch
            {

            }
        }

        public async Task<GeneralResponse> CreateRoleAsync(CraeteRoleDto model)
        {
            var role = new IdentityRole() { Name = model.Name };
            var res = await roleManager.CreateAsync(role);
            if (res.Succeeded) return new GeneralResponse(true, "Role Created succesfull");
            return new GeneralResponse(false, "Faild Created Role");
        }

        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync()
         => (await roleManager.Roles.ToListAsync()).Adapt<IEnumerable<GetRoleDTO>>();

        public async Task<IEnumerable<GetUserWithRolesDTo>> GetUsersWithRoleAsync()
        {
            var allUsers = await userManager.Users.ToListAsync();
            if (allUsers == null) return null;
            var ListOfRoles = new List<GetUserWithRolesDTo>();
            foreach (var user in allUsers)
            {
                var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                var RoleInfo = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == userRole.ToLower());
                ListOfRoles.Add(new GetUserWithRolesDTo()
                {
                    Email = user.Email,
                    Name = user.Name,
                    RoleID = RoleInfo.Id,
                    RoleName = userRole,
                    UserID = user.Id
                });
            }
            return ListOfRoles;
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var user = await FindUserByEmailAsync(model.Email);
                if (user == null) return new LoginResponse(false, "User Name or Password invaild ");
                Microsoft.AspNetCore.Identity.SignInResult signInResult;
                try
                {
                    signInResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch (Exception ex)
                {
                    return new LoginResponse(false, "User Name or Password invaild");
                }

                if (!signInResult.Succeeded)
                {                    if (!user.EmailConfirmed)
                    {
                        return new LoginResponse(false, "You need To Confirm Email");
                    }

                    return new LoginResponse(false, "User Name or Password invaild");
                }
                string token = await GenerateToken(user);
                string refreshToken = GenerateRefreshToken();
                if (token is null || refreshToken is null) return new LoginResponse(false, "User Name or Password invaild");
                var saveResult = await SaveRefreshToken(user.Id, refreshToken);
                if (saveResult.flag)
                    return new LoginResponse { flag = true, message = "Success", Token = token, RefreshToken = refreshToken,userId=user.Id };
                return new LoginResponse { flag = false, message = saveResult.message };
            }
            catch (Exception ex)
            {
                return new LoginResponse(false, ex.Message);
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(string Retoken)
        {
            
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == Retoken);
            if (token == null) return new LoginResponse();
            var user = await userManager.FindByIdAsync(token.UserID);
            if (user == null) return new LoginResponse(false, "error");
            var newToken = await GenerateToken(user);
            if (!(token.ExpiryDate > DateTime.Now.AddDays(1)))
            {
            var refreshToken = GenerateRefreshToken();
            var res = await SaveRefreshToken(user.Id, refreshToken);
            if (!res.flag) return new LoginResponse(false, res.message);
            return new LoginResponse(true, res.message, newToken, refreshToken,user.Id);

            }
            return new LoginResponse(true, "successfully", newToken, token.Token, user.Id);



        }
        private async Task<GeneralResponse> SaveRefreshToken(string userID, string token)
        {
            try
            {
                var user = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.UserID == userID);
                if (user == null)
                    await _context.RefreshTokens.AddAsync(new RefreshTocken() { UserID = userID, Token = token, ExpiryDate = DateTime.Now.AddDays(7), IsRevoked = false });

                else
                {
                    user.Token = token;
                    user.ExpiryDate = DateTime.Now.AddDays(7);
                }
                await _context.SaveChangesAsync();
                return new GeneralResponse { flag = true };
            }
            catch (Exception ex)
            {
                return new GeneralResponse { flag = true, message = ex.Message };
            }
        }




        //remove in after Development
        public async Task SendEmail(string userId)
        {
            var user= await userManager.FindByIdAsync(userId);
            await SendConfirmationEmail(user);
        }

        private async Task SendConfirmationEmail(ApplicationUser userModel)
        {

            var token = await userManager.GenerateEmailConfirmationTokenAsync(userModel);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));//WebUtility.UrlEncode(token);
            var requestAccessor = _contextAccessor.HttpContext.Request;
            //api/Account/ConfirmEmail
            var reqest = requestAccessor.Scheme + "://" + requestAccessor.Host + "/Template/confirmed.html?userId=" + userModel.Id + "&token=" + encodedToken;
            var filePath = "wwwroot/Template/emailConfirm.html";
            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("Url_aboshaban", reqest);


            var sendMessage = await _emailService.SendEmailAsync(userModel.Email, "Please Confirm Your Email", mailText);





        }

        public async Task<GeneralResponse> ConfirmEmail(string userID, string Token)
        {
            var user = await userManager.FindByIdAsync(userID);
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Token)); // WebUtility.UrlDecode(Token);
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                return new GeneralResponse(true, "Confirmed Email successful");
            }
            string err = "";
            foreach (var item in result.Errors)
            {
                err += item.Description;
            }
            return new GeneralResponse { flag = false, message = err };

        }


        public async Task<GeneralResponse> RemoveUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new GeneralResponse(false,"User not Found");
               
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new GeneralResponse(true, "User deleted successfully");
            }
            string error="";
            foreach (var item in result.Errors)
            {
                error += item.Description;
            }
            return new GeneralResponse() {flag=false, message = error };
        }


        public async Task<GeneralResponse> LogOut(string userId)
        {
              
            var refreshTokens = await _context.RefreshTokens
                                     .Where(t => t.UserID == userId )
                                     .FirstOrDefaultAsync();
            _context.RefreshTokens.Remove(refreshTokens);
            await signInManager.SignOutAsync();
             return new GeneralResponse(true, "User deleted successfully");
        }




        public async Task<GeneralResponse> ResetPassword(string userId,string token,string password)
        {
            ApplicationUser userModel = await userManager.FindByIdAsync(userId);
            if (userModel != null)
            {
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
                var result = await userManager.ResetPasswordAsync(userModel, decodedToken, password);
                if (result.Succeeded)
                {
                    return new GeneralResponse(true,"password changes");
                }
                return new GeneralResponse(false, "Invalid password");
            }


            return new GeneralResponse(false, "Invalid user ID");

        }




        public async Task ForgetPassword(string userEmail, string origion)
        {
            var userModel= await FindUserByEmailAsync(userEmail);

            if (userModel != null)
            {

                var token = await userManager.GeneratePasswordResetTokenAsync(userModel);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));//WebUtility.UrlEncode(token);
               // var requestAccessor = _contextAccessor.HttpContext.Request;
              // requestAccessor.Scheme + "://" + requestAccessor.Host 
                var reqest = origion + "/ResetPassword?userId=" + userModel.Id + "&token=" + encodedToken;
                var filePath = "wwwroot/Template/emailConfirm.html";
                var str = new StreamReader(filePath);

                var mailText = str.ReadToEnd();
                str.Close();

                mailText = mailText.Replace("Url_aboshaban", reqest);


                var sendMessage = await _emailService.SendEmailAsync(userModel.Email, "Please Change Your Password", mailText);
            }



        }

        public async Task<GeneralResponse> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            
                ApplicationUser userModel = await userManager.FindByNameAsync(changePasswordDTO.Email);

                if (userModel != null && await userManager.CheckPasswordAsync(userModel, changePasswordDTO.Password))
                {
                    await userManager.ChangePasswordAsync(userModel, changePasswordDTO.Password, changePasswordDTO.NewPassword);


                         return new GeneralResponse(true, "Password Change successfully");

                }

            return new GeneralResponse(true, "invaild Password Or User");

        }

        public async Task<GeneralResponse> emailExists(string email)
        {
          var user=  await FindUserByEmailAsync(email);
            if(user != null)
            {
                return new GeneralResponse(true, "Email Found");

            }
            else
            {
                return new GeneralResponse(false, "Email Not Found");
            }
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return true;
            }
            return false;

        }

       public async Task<string> GetRoleOfUser(string userId)
        {
            var user=await userManager.FindByIdAsync(userId);
            string role = (await userManager.GetRolesAsync(user)).FirstOrDefault().ToString();
            return role;
        }
    }
}
