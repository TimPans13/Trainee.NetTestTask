using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;

namespace TestTask.Services.Interfaces
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUser()
        {
            try
            {
                var userWithMostOrders = await _dbContext.Users
                    .Include(u => u.Orders)
                    .Where(u => u.Orders.Any())
                    .OrderByDescending(u => u.Orders.Count)
                    .FirstOrDefaultAsync();

                return userWithMostOrders;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetUser: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                var inactiveUsers = await _dbContext.Users
                    .Include(u => u.Orders)
                    .Where(u => u.Status == UserStatus.Inactive)
                    .ToListAsync();

                return inactiveUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetUsers: {ex.Message}");
                throw;
            }
        }
    }
}
