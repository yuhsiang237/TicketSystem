namespace TicketSystem.Models
{
    public class RspOrderTicket
    {
        public string TicketNumber { get; set; }
        public DateTime? DateTime { get; set; }
        public bool IsOK { get; set; }
    }
}
