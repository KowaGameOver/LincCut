using System.ComponentModel.DataAnnotations;

namespace LincCut.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public Roles Role { get; set; } = Roles.guest;
        public string? Password { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
