namespace Application.DTOs.Request.Management
{
    public class CreateCategoryRequest
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? UserId { get; set; }
    }
}