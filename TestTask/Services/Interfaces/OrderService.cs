using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;

namespace TestTask.Services.Interfaces
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrder()
        {
            var orderWithHighestTotalAmount = await _context.Orders
                .OrderByDescending(o => o.Price * o.Quantity)
                .FirstOrDefaultAsync();

            return orderWithHighestTotalAmount;
        }

        public async Task<List<Order>> GetOrders()
        {

            var ordersWithQuantityGreaterThanTen = await _context.Orders
                .Where(o => o.Quantity > 10)
                .ToListAsync();

            return ordersWithQuantityGreaterThanTen;
        }
    }
}
