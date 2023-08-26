namespace LincCut.Dto
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string? EMAIL { get; set; } = string.Empty;
        public string? PASSWORD { get; set; } = string.Empty;
        public string? TOKEN { get; set; } = string.Empty;
        public Roles ROLE { get; set; } = Roles.guest;

    }
}
