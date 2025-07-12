using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        public string Password { get; set; }
    }
}
