using System.ComponentModel.DataAnnotations;

namespace LincCut.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public Roles Role { get; set; } = Roles.guest;
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null;
        public DateTime LastLogin { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
