using System.ComponentModel.DataAnnotations;

namespace Edutrack.Models;

public class t_Login
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; }
}