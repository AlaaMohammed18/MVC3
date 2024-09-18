using System.ComponentModel.DataAnnotations;

namespace Company.Web.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email Is Required")]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
