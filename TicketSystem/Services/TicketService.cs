using TicketSystem.Models;
using TicketSystem.Services._Interfaces;

namespace TicketSystem.Services
{
    /// <inheritdoc/>
    public class TicketService : ITicketService
    {
        private static Dictionary<string, TicketModel>? _tickets = null;

        private Dictionary<string, object> _ticketlocks = new Dictionary<string, object>();

        /// <summary>
        /// constructor
        /// </summary>
        public TicketService()
        {
            InitTicket();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<RspOrderTicket> OrderTicket(ReqOrderTicket req)
        {
            var now = DateTime.Now;
            var result = new RspOrderTicket { };

            // 獲取票號對應的鎖
            object ticketLock;
            if (!_ticketlocks.TryGetValue(req.TicketNumber, out ticketLock))
            {
                throw new ArgumentException($"Ticket number {req.TicketNumber} not found.");
            }

            // 使用細顆粒的票鎖來避免競爭
            lock (ticketLock)
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

                // 初始化每個票號對應的鎖
                foreach (var ticketNumber in _tickets.Keys)
                {
                    _ticketlocks.Add(ticketNumber, new object());
                }
            }
        }
    }
}
