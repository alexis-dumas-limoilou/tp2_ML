using System.ComponentModel.DataAnnotations;

namespace appClient.Models
{
    public class Image
    {
        [Display(Name = "Image à téléverser")]
        [FileExtensions(Extensions ="png, gif, jpeg, jpg, webp", ErrorMessage = "Veuillez télécharger un fichier avec une extension valide : png, gif, jpeg, jpg ou webp.")]
        public byte[] ImageData { get; set; }
    }
}
