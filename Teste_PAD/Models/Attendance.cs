using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teste_PAD.Models
{
    public class Attendance
    {
        [Key]
        [Column(Order = 1)]
        public int EventId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int UserId { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
    }
}
