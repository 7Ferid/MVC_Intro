using System.ComponentModel.DataAnnotations;

namespace MVC_Intro.ViewModels.UserViewModels
{
    public class RegisterVM
        
    {
        [Required, MaxLength(32), MinLength(3)]
        public string UserName { get; set; } = string.Empty;
        [Required,MaxLength(32),MinLength(3)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(32), MinLength(3)]
        public string LastName { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(3),EmailAddress]
        public string EmailAdress { get; set; }= string.Empty;
        [Required, MaxLength(256), MinLength(3),DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(3), DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword {  get; set; } = string.Empty;

    }


}
