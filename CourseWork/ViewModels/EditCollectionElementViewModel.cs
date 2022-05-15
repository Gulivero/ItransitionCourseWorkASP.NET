using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels
{
    public class EditCollectionElementViewModel
    {
        [Required] 
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")] 
        public string Name { get; set; }

        [Required]
        [Display(Name = "Теги")]
        public string Tags { get; set; }

        [Display(Name = "Картинка")]
        public string Image { get; set; }
    }
}