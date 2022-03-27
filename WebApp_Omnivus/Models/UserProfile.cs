using System.ComponentModel.DataAnnotations;

namespace WebApp_Omnivus.Models
{
    public class UserProfile
    {
        public string UserId { get; set; } = string.Empty;
        [Display(Name = "First name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^([a-öA-Ö]+?)([-][a-öA-Ö]+)*?$", ErrorMessage = "Must be a valid first name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^([a-öA-Ö]+?)([-\s][a-öA-Ö]+)*?$", ErrorMessage = "Must be a valid last name")]
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Street name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^([a-öA-Ö]+?)([\s][0-9]+)*?$", ErrorMessage = "Must be a valid streetname")]
        public string StreetName { get; set; } = string.Empty;

        [Display(Name = "Postal code")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain 5 digits and a whitespace", MinimumLength = 5)]
        [RegularExpression(@"^\d{3}(\s\d{2})?$", ErrorMessage = "Must be a valid postal code (eg. 123 45)")]
        public string PostalCode { get; set; } = string.Empty;

        [Display(Name = "City")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^([a-öA-Ö]+?)([\s][a-öA-Ö]+)*?$", ErrorMessage = "Must be a valid name")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Country")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(256, ErrorMessage = "Must contain at least 2 characters", MinimumLength = 2)]
        [RegularExpression(@"^([a-öA-Ö]+?)([\s][a-öA-Ö]+)*?$", ErrorMessage = "Must be a valid name")]
        public string Country { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = "";
        public string DisplayName => $"{FirstName} {LastName}";
        
    }
}
