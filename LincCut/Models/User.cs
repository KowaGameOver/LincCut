using System.ComponentModel.DataAnnotations;

namespace LincCut.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public Roles Role { get; set; } = Roles.guest;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
