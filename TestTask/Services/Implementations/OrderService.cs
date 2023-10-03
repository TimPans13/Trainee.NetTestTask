using System;
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
            try
            {
                var orderWithHighestTotalAmount = await _context.Orders
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
                    .Where(o => o.Quantity > 10)
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
