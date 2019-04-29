using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoBlazor.Shared.Authorize
{
    public class TokenViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
