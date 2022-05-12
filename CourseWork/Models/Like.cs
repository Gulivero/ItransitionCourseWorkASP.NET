namespace CourseWork.Models
{
    public class Like
    {
        public int Id { get; set; }
        public User User { get; set; }
        public CollectionElement CollectionElement { get; set; }
    }
}
