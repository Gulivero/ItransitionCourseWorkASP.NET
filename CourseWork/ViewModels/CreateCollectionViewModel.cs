using CourseWork.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels
{
    public class CreateCollectionViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Название коллекции")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Картинка")]
        public string Image { get; set; }

        [Display(Name = "Тема")]
        public Theme Theme { get; set; }

        [Display(Name = "Дополнительные поля для элементов")]
        public List<AdditionalFieldName> AdditionalFieldNames { get; set; }
    }
}
