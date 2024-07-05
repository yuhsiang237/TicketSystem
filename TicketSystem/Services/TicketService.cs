using TicketSystem.Models;
using TicketSystem.Services._Interfaces;

namespace TicketSystem.Services
{
    public class TicketService : ITicketService
    {
        private static Dictionary<string, TicketModel>? _tickets = null;
        private static readonly object _lockTicket = new object();

        public TicketService()
        {
            InitTicket();
        }

        public async Task<List<RspGetAllTicket>> GetAllTicket()
        {
            var result = new List<RspGetAllTicket>();
            foreach (var item in _tickets)
            {
                var ticket = item.Value;
                result.Add(new RspGetAllTicket
                {
                    TicketNumber = ticket.TicketNumber,
                    IsPurchase = ticket.PurchaseDate != null,
                });
            }
            return await Task.FromResult(result);
        }

        public async Task<List<RspGetUnPurchasedTicket>> GetUnPurchasedTicket()
        {
            var result = new List<RspGetUnPurchasedTicket>();
            foreach (var item in _tickets)
            {
                var ticket = item.Value;
                if (ticket.PurchaseDate == null)
                {
                    result.Add(new RspGetUnPurchasedTicket
                    {
                        TicketNumber = ticket.TicketNumber,
                    });
                };
            }
            return await Task.FromResult(result);
        }

        public async Task<RspOrderTicket> OrderTicket(ReqOrderTicket req)
        {
            var now = DateTime.Now;
            var result = new RspOrderTicket { };
            lock (_lockTicket)
            {
                if (_tickets.TryGetValue(req.TicketNumber, out TicketModel ticket))
                {
                    if (ticket.PurchaseDate != null)
                    {
                        result = new RspOrderTicket
                        {
                            TicketNumber = req.TicketNumber,
                            DateTime = now,
                            IsOK = false
                        };
                    }
                    else
                    {
                        ticket.PassengerName = req.PassengerName ?? string.Empty;
                        ticket.PurchaseDate = now;

                        _tickets[req.TicketNumber] = ticket;

                        result = new RspOrderTicket
                        {
                            TicketNumber = ticket.TicketNumber,
                            DateTime = now,
                            IsOK = true
                        };
                    }
                }
                else
                {
                    result = new RspOrderTicket
                    {
                        DateTime = now,
                        IsOK = false
                    };
                }
            }

            return await Task.FromResult(result);
        }

        private void InitTicket()
        {
            if (_tickets == null)
            {
                _tickets = new Dictionary<string, TicketModel>();
                for (var i = 1; i <= 1000; i++)
                {
                    _tickets.Add($"T-{i}", new TicketModel
                    {
                        PassengerName = string.Empty,
                        TicketNumber = $"T-{i}",
                        PurchaseDate = null
                    });
                }
            }
        }
    }
}
