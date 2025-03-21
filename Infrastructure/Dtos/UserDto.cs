namespace Infrastructure.Dtos;

public class UserDto
{
    public class UserCreateDto
    {
        public required string Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public required string Password { get; set; }
    }
}
