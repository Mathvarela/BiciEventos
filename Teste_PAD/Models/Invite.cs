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

        public bool IsRead { get; set; }

        public override string ToString()
        {
            return $"{Inviter.Username} - {Event.Description}";
        }
    }
}
