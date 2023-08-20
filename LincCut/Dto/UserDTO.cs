namespace LincCut.Dto
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? Token { get; set; } = null;
        public Roles Role { get; set; } = Roles.guest;

    }
}
