using System.ComponentModel.DataAnnotations;

namespace Company.Web.Models
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
    }
}
