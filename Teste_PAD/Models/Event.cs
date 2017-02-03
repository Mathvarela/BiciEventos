using System;

namespace Teste_PAD.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StartLatitude { get; set; }
        public double EndLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLongitude { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
