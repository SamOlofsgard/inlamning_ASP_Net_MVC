using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp_Omnivus.Models.Entities
{
    public class ProfileImageEntity
    {
        [Key]
        public string FileName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string UserId { get; set; }
    }
}
