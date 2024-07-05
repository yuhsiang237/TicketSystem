using Microsoft.AspNetCore.Mvc;
using TicketSystem.Models;
using TicketSystem.Services._Interfaces;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketService"></param>
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// �ʶR��
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("OrderTicket")]
        public async Task<RspOrderTicket> OrderTicket([FromBody] ReqOrderTicket req)
        {
            return await _ticketService.OrderTicket(req);
        }


        /// <summary>
        /// ���o���ʶR����
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUnPurchasedTicket")]
        public async Task<List<RspGetUnPurchasedTicket>> GetUnPurchasedTicket()
        {
            return await _ticketService.GetUnPurchasedTicket();
        }

        /// <summary>
        /// ���o�Ҧ�����
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllTicket")]
        public async Task<List<RspGetAllTicket>> GetAllTicket()
        {
            return await _ticketService.GetAllTicket();
        }
    }
}