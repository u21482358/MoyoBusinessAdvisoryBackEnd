using System.ComponentModel.DataAnnotations;

namespace MoyoBusinessAdvisory.ViewModels
{
    public class GoogleSignInVM
    {

        [Required]
        public string Email { get; set; }

        public string IdToken { get; set; }
    }
}
