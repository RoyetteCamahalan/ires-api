namespace ires_api.DTO.User
{
    public class UserLoginRequestDto
    {
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string key { get; set; } = string.Empty;
    }
}
