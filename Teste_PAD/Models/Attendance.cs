namespace Teste_PAD.Models
{
    public class Attendance
    {
        public int UserId { get; set; }
        public int EventId { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
    }
}
