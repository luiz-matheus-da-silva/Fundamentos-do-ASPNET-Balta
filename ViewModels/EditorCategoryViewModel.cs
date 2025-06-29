using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 30, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres.")]
        public string Name { get; set; }
    }
}
