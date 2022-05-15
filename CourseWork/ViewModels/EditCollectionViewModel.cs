using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels
{
    public class EditCollectionViewModel
    {
        [Required] 
        public int Id { get; set; }

        [Required] 
        [Display(Name = "Название")] 
        public string Name { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Картинка")]
        public string Image { get; set; }

    }
}