using System.ComponentModel.DataAnnotations;

namespace FinalProject.Model.Dto;

public class UserDto
{
    [Required] [MaxLength(256)] public string Name { get; set; }
    [Required] [MaxLength(256)] public string UserName { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
}