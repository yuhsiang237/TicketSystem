using TicketSystem.Models;

namespace TicketSystem.Services._Interfaces
{
    public interface ITicketService
    {
        /// <summary>
        /// 取得剩餘票數
        /// </summary>
        /// <returns></returns>
        public Task<List<RspGetUnPurchasedTicket>> GetUnPurchasedTicket();

        /// <summary>
        /// 取得全部票數
        /// </summary>
        /// <returns></returns>
        public Task<List<RspGetAllTicket>> GetAllTicket();

        /// <summary>
        /// 買票
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<RspOrderTicket> OrderTicket(ReqOrderTicket req);
    }
}
