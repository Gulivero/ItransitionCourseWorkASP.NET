using System.Collections.Generic;

namespace CourseWork.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Theme Theme { get; set; }
        public User User { get; set; }
        public List<AdditionalFieldName> AdditionalFieldsNames { get; set; }
        public List<CollectionElement> Elements { get; set; }
    }
}
