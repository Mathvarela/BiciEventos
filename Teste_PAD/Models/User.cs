using System;
using System.Collections.Generic;

namespace Teste_PAD.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime RegisterDate { get; set; }
        public IEnumerable<Invite> InvitesReceived { get; set; }
        public IEnumerable<Invite> InvitesSent { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Attendance> Attendances { get; set; }
    }
}
