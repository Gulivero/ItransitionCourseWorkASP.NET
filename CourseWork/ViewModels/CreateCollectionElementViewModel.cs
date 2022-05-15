using CourseWork.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels
{
    public class CreateCollectionElementViewModel
    {
        [Required]
        public int CollectionId { get; set; }

        [Required]
        [Display(Name = "Название предмета")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Дополнительные поля")]
        public List<AdditionalField> AdditionalFields { get; set; } = new ();

        [Required]
        [Display(Name = "Теги")]
        public string Tags { get; set; }

        [Display(Name = "Картинка")]
        public string Image { get; set; }
    }
}
