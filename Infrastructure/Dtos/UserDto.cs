namespace Infrastructure.Dtos;

public class UserDto
{
    public class UserRegisterDto
    {
        public uint UserId { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }

    public class UserSetNewPasswordForUserDto
    {
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }

    public class UserCredentialsDto
    {
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
    }
}
