using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels
{
    public class CommentViewModel
    {
        [Required] 
        public string UserId { get; set; }
        [Required] 
        public int CollectionElementId { get; set; }
        [Required] 
        [Display(Name = "Текст комментария")] 
        public string Text { get; set; }
    }
}