namespace CourseWork.Models
{
    public class AdditionalField
    {
        public int Id { get; set; }
        public AdditionalFieldName Name { get; set; }
        public string Value { get; set; }
        public CollectionElement CollectionElement { get; set; }
    }
}
