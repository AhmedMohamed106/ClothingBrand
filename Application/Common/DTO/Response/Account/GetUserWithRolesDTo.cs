

namespace Application.DTOs.Response.Account
{
    public class GetUserWithRolesDTo
    {
        public string Name {  get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public string RoleID { get; set; }
        public string UserID { get; set; }
    }
}
