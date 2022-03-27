using System.ComponentModel.DataAnnotations;

namespace WebApp_Omnivus.Models.Forms
{
    public class ProfileImageUploadForm
    {
        [Required]
        [Display(Name = "Profileimage:")]
        public IFormFile File { get; set; }
    }
}
