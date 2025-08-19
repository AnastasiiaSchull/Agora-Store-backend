using System.ComponentModel.DataAnnotations;

namespace Agora.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
