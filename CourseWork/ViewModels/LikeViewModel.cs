using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels
{
    public class LikeViewModel
    {
        [Required] 
        public string UserId { get; set; }
        [Required] 
        public int CollectionElementId { get; set; }
    }
}