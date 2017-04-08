namespace DotW.Models
{
    public enum RoleTypes
    {
        Admin = 1,
        User = 2
    }

    public class UserRoles
    {
        public string RoleName { get; set; }
        public string RoleValue { get; set; }
    }
}