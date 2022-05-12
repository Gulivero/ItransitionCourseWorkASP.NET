namespace CourseWork.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public User User { get; set; }
        public CollectionElement CollectionElement { get; set; }
        public string Text { get; set; }
    }
}
