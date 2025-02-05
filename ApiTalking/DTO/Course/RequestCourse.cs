namespace DTOs.Course
{
    public class CourseRequestDTO
    {
        public required string Name { get; set; }  
        public string? Description { get; set; }
        public int UserId { get; set; }  
    }
}
