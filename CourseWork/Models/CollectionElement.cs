using System.Collections.Generic;

namespace CourseWork.Models
{
    public class CollectionElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Collection Collection { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public List<Tag> Tags { get; set; }
        public List<AdditionalField> AdditionalFields { get; set; }
    }
}
