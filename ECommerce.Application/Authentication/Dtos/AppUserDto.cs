namespace ECommerce.Application.Authentication.Dtos
{
    public class AppUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}
