using TicketSystem.Models;
using TicketSystem.Services;
using TicketSystem.Services._Interfaces;
using Xunit;

namespace TicketSystemTest.Services
{
    public class TicketServiceTest
    {
        private ITicketService _ticketService;
        public TicketServiceTest()
        {
            _ticketService = new TicketService();
        }

        [Fact]
        public async void GetAllTicket()
        {
            var result = await _ticketService.GetAllTicket();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async void GetUnPurchasedTicket()
        {
            var result = await _ticketService.GetUnPurchasedTicket();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void OrderTicket()
        {
            // 同時啟動多個訂票任務
            var tasks = new List<Task<RspOrderTicket>>();
            for (int i = 0; i < 80000; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var result = await _ticketService.OrderTicket(new ReqOrderTicket
                    {
                        TicketNumber = "T-1",
                        PassengerName = $"Passenger-{Guid.NewGuid()}"
                    });
                    return result;
                }));
            }

            // 等待所有訂票任務完成
            Task.WaitAll(tasks.ToArray());

            // 檢查是否有多個訂票成功的情況
            int successfulOrders = tasks.Count(t => t.Result.IsOK);
            Assert.Equal(1, successfulOrders); // 預期只有一個訂票成功，其餘應該因為競態條件而失敗
        }
    }
}
