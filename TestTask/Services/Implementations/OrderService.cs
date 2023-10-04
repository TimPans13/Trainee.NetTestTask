using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;

namespace TestTask.Services.Interfaces
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private const int minimumQuantity = 10;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrder()
        {
            try
            {
                var orderWithHighestTotalAmount = await _context.Orders
                    .Include(u => u.User)
                    .OrderByDescending(o => o.Price * o.Quantity)
                    .FirstOrDefaultAsync();

                return orderWithHighestTotalAmount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetOrder: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Order>> GetOrders()
        {
            try
            {
                var ordersWithQuantityGreaterThanTen = await _context.Orders
                    .Where(o => o.Quantity > minimumQuantity)
                    .Select(o => new Order
                    {
                        Id = o.Id,
                        ProductName = o.ProductName,
                        Price = o.Price,
                        Quantity = o.Quantity,
                        UserId = o.UserId,
                        User = o.User != null && o.User.Orders.Any() ? new User
                        {
                            Id = o.User.Id,
                            Email = o.User.Email,
                            Status = o.User.Status,
                        } : null
                    })
                    .ToListAsync();

                return ordersWithQuantityGreaterThanTen;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetOrders: {ex.Message}");
                throw;
            }
        }
    }
}
