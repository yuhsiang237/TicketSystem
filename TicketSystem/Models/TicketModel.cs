namespace TicketSystem.Models
{
    public class TicketModel
    {
        public string TicketNumber { get; set; }
        public string PassengerName { get; set; }
        public string SeatNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
}
