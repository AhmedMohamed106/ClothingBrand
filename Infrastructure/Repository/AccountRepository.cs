

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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace infrastructure.Repos
{
    public class AccountRepository: IAccount
    {
        private RoleManager<IdentityRole> roleManager;

        private UserManager<ApplicationUser> userManager;
        private IConfiguration config;
        private ApplicationDbContext _context;
        private SignInManager<ApplicationUser> signInManager;

      public AccountRepository(RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager, IConfiguration config, ApplicationDbContext context,
        SignInManager<ApplicationUser> signInManager)
        {
            this.roleManager = roleManager; 
            this.userManager = userManager;
            this.config = config;
            this.signInManager = signInManager;
            this._context = context;

        }
        private async Task<ApplicationUser> FindUserByEmailAsync(string email) => await userManager.FindByEmailAsync(email);
        private async Task<IdentityRole> FindRoleByNameAsync(string user) => await roleManager.FindByNameAsync(user);

        private static string GenerateRefreshToken()=>Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)); 

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]));
                var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
                var userClaims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),

                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, (await userManager.GetRolesAsync(user)).FirstOrDefault().ToString()),
                    new Claim("FullName", user.Name)
                };





               var token= new JwtSecurityToken(
                    issuer: config["Jwt:ValidIssuer"],
                    audience: config["Jwt:ValidAudiance"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                     ); 
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch 
            {
                return null!;
            
            }
        }




        private async Task<GeneralResponse> AssignUserToRoleAsync(ApplicationUser user,IdentityRole role)
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
           var user= await FindUserByEmailAsync(model.UserEmail);
            var role=await FindRoleByNameAsync(model.RoleName);
            if (user == null) return new GeneralResponse(false, "user not Found");
            if (role == null) return new GeneralResponse(false, "role not Found");
            var PreviousRole= (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var RemoveOldRole =await userManager.RemoveFromRoleAsync(user,PreviousRole);
            if (!RemoveOldRole.Succeeded)return new GeneralResponse(false, RemoveOldRole.Errors.ToString());
            var newRole=await userManager.AddToRoleAsync(user,role.Name);
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
                var result= await userManager.CreateAsync(user,model.Password);
                if (!result.Succeeded)
                {
                    string error = "";
                    foreach(var err in  result.Errors) {
                        error += err.Description;
                    }

                    return new GeneralResponse(false, error);   

                }
              return  await AssignUserToRoleAsync(user,new IdentityRole() { Name=model.Role });


            }
            catch(Exception ex) 
            { 
                return new GeneralResponse( false ,ex.Message);
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
                    Password = "ahmed@123",
                    Name = "ahmed Shaban",
                    Role = "admin"
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
                if (res.Succeeded) return new GeneralResponse(true,"Role Created succesfull");
            return new GeneralResponse(false, "Faild Created Role");
        }

        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync()
         => (await roleManager.Roles.ToListAsync()).Adapt<IEnumerable<GetRoleDTO>>();   

        public async Task<IEnumerable<GetUserWithRolesDTo>> GetUsersWithRoleAsync()
        {
            var allUsers=await userManager.Users.ToListAsync();
            if (allUsers == null) return null;
            var ListOfRoles=new List<GetUserWithRolesDTo>();
            foreach (var user in allUsers)
            {
                var userRole=(await userManager.GetRolesAsync(user)).FirstOrDefault();
                var RoleInfo = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == userRole.ToLower());
                ListOfRoles.Add(new GetUserWithRolesDTo()
                {
                    Email = user.Email,
                    Name = user.Name,
                    RoleID = RoleInfo.Id,
                    RoleName = userRole
                });
            }
            return ListOfRoles;
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var user = await FindUserByEmailAsync(model.Email);
                if (user == null) return new LoginResponse(false, "invalid Login");
                SignInResult signInResult;
                try
                {
                    signInResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch (Exception ex)
                {
                    return new LoginResponse(false, "invalid Login");
                }
                if (!signInResult.Succeeded) return new LoginResponse(false, "invalid Login");
                string token = await GenerateToken(user);
                string refreshToken = GenerateRefreshToken();
                if(token is null||refreshToken is null) return new LoginResponse(false, "invalid Login");
                var saveResult=await SaveRefreshToken(user.Id,refreshToken);
                if(saveResult.flag)
                    return new LoginResponse { flag = true, message = "Success", Token = token, RefreshToken = refreshToken };
                return new LoginResponse { flag = false,message=saveResult.message};
            }
            catch(Exception ex) 
            {
                return new LoginResponse(false,ex.Message);
            }
        }

        public async Task<LoginResponse>RefreshTokenAsync(RefreshTockenDto model)
        {
            var token =await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == model.Token);
            if(token == null) return new LoginResponse();
            var user = await userManager.FindByIdAsync(token.UserID);
            //if (user == null) return new LoginResponse(false, "error");
           var newToken=await GenerateToken(user);
            var refreshToken =  GenerateRefreshToken();
            var res=await SaveRefreshToken(user.Id, refreshToken);
            if (!res.flag) return new LoginResponse();
            return new LoginResponse (  true, res.message,newToken,refreshToken);



        }
        private async Task<GeneralResponse> SaveRefreshToken(string userID,string token)
        {
            try
            {
                var user = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.UserID == userID);
                if (user == null)
                    await _context.RefreshTokens.AddAsync(new RefreshTocken() { UserID = userID, Token = token });
                else
                    user.Token = token;
                await _context.SaveChangesAsync();
                return new GeneralResponse { flag = true };
            }
            catch (Exception ex)
            {
                return new GeneralResponse { flag = true, message = ex.Message };
            }
        }
    }
}
