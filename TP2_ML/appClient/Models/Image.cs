using System.ComponentModel.DataAnnotations;

namespace appClient.Models
{
    public class Image
    {
        [Display(Name = "Image à téléverser")]
        [Required]
        public IFormFile ImageData { get; set; }
    }
}
