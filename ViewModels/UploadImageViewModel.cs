using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "O arquivo é obrigatório.")]
        public string Base64Image { get; set; }
    }
}
