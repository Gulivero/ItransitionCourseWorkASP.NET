using System.Collections.Generic;

namespace CourseWork.Models
{
    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Collection> Collections { get; set; }
    }
}
