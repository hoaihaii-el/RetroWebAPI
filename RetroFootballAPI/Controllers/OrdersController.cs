using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RetroFootballAPI.Hubs;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repo;
        private readonly IHubContext<OrderStatusHub> _hub;

        public OrdersController(IOrderRepo repo, IHubContext<OrderStatusHub> hub)
        {
            _repo = repo;
            _hub = hub;
        }

        
        [HttpGet("get-orders")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int orderType = 0,
            [FromQuery] int month = 0,
            [FromQuery] string customerID = "",
            [FromQuery] bool today = false)
        {
            return Ok(await _repo.GetOrders(orderType, month, customerID, today));
        }

        
        [HttpGet("get-by-customerID/{customerID}/{type}")]
        [Authorize]
        public async Task<ActionResult> GetByCustomer(string customerID, int type)
        {
            return Ok(await _repo.GetByCustomer(customerID, type));
        }

        
        [HttpPost("new-order")]
        [Authorize]
        public async Task<IActionResult> Add([FromForm] OrderVM order)
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    data = await _repo.Add(order)
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        
        [HttpPut("update-status/{orderID}")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int orderID)
        {
            var order = await _repo.UpdateStatus(orderID);

            if (OrderStatusHub.userConnections.ContainsKey(order.CustomerID ?? ""))
            {
                await _hub.Clients.Client(OrderStatusHub.userConnections[order.CustomerID ?? ""])
                        .SendAsync("ReceiveMessage", order);
            }

            return Ok(new
            {
                success = true,
                data = order
            });
        }


        [HttpPost("update-payment-status/{orderID}")]
        public async Task<IActionResult> UpdatePayment(int orderID)
        {
            return Ok(new
            {
                success = true,
                data = await _repo.UpdatePaymentStatus(orderID)
            });
        }


        [HttpDelete("cancel-order/{orderID}/{isCancelByAdmin}")]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int orderID, bool isCancelByAdmin)
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    order = await _repo.Cancel(orderID, isCancelByAdmin)
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}
