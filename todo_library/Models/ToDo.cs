namespace todo_library.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;

    }
}
