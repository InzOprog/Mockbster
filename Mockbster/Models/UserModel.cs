using System.ComponentModel.DataAnnotations;
namespace Mockbster.Models;

public class UserModel
{
    public int Id { get; set; }
    [StringLength(20, MinimumLength = 3)]
    [Required]
    public string? Firstname { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string? Lastname { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*$")]
    [Required]
    public string? Email { get; set; }
    [StringLength(20, MinimumLength = 3)]
    [Required]
    public string? Username { get; set; }
    // Password stored in clear text === best security practice
    [StringLength(20, MinimumLength = 3)]
    [Required]
    public string? Password { get; set; }
}
