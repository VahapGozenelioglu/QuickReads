using System.ComponentModel.DataAnnotations;

namespace QuickReads.Entities.DTOs;

public class UserDto
{
    [Required]public string Email { get; set; }
    [Required]public string Password { get; set; }
}

public class UserSignUpDto : UserDto
{
    [Required]public string FirstName { get; set; }
    [Required]public string LastName { get; set; }
}

public class UserLoginDto : UserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}