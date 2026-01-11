using System.ComponentModel.DataAnnotations;

namespace MVC_Intro.ViewModels.UserViewModels
{
    public class LoginVM {

        [Required, MaxLength(256), MinLength(3), EmailAddress]
        public string EmailAdress { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(3), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool IsRemember { get; set; }

        public string? ReturnUrl { get; set; }

    }


}
