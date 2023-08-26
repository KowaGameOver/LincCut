using System.ComponentModel.DataAnnotations;

namespace LincCut.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string? EMAIL { get; set; } = string.Empty;
        public Roles ROLE { get; set; } = Roles.guest;
        public string? PASSWORD { get; set; } = string.Empty;
        public DateTime LAST_LOGIN_DATE { get; set; }
        public DateTime REGISTRATION_DATE { get; set; }
    }
}
