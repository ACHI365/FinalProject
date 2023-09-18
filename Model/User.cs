using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required] [MaxLength(256)] public string Name { get; set; } = string.Empty;
    [Required] [MaxLength(256)] public string UserName { get; set; } = string.Empty;
    [Required] public string PasswordHash { get; set; } = string.Empty;
    [Required] public string VerificationCode { get; set; } = string.Empty;
    [Required] public string RegistrationMethod { get; set; } = String.Empty;
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] [EmailAddress] public string ResetPasswordToken { get; set; } = string.Empty;
    [Required] [EmailAddress] public DateTime ResetPasswordTokenExpiry { get; set; }
    [Required] public Role Role { get; set; }
    [Required] public bool IsVerified { get; set; }
}