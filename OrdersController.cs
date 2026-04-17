using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _context.Orders.ToList();
            return Ok(orders);
        }

        // POST: api/orders
        [HttpPost]
        public IActionResult AddOrder(Order order)
        {
            order.OrderDate = DateTime.Now;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok(order);
        }
    }
}