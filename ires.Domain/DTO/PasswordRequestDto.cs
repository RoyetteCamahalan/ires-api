namespace ires.Domain.DTO
{
    public class PasswordRequestDto
    {
        public string userpass { get; set; } = string.Empty;
        public string newuserpass { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
    }
}
