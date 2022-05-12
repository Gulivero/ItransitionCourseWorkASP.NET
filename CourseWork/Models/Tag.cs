using System.Collections.Generic;

namespace CourseWork.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CollectionElement> CollectionElements { get; set; }
    }
}
