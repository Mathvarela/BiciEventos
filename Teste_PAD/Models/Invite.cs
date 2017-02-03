namespace Teste_PAD.Models
{
    public class Invite
    {
        public int EventId { get; set; }
        public int InviterId { get; set; }
        public int InvitedId { get; set; }

        public Event Event { get; set; }
        public User Inviter { get; set; }
        public User Invited { get; set; }
    }
}
