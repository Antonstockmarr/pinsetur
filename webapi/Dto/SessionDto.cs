namespace stockmarrdk_api.Dto
{
    public class SessionDto
    {
        public string Jwt { get; set; } = "";
        public UserDto User { get; set; } = new UserDto();
    }
}
