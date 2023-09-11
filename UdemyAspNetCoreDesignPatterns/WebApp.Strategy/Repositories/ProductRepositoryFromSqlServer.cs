using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repositories
{
    public class ProductRepositoryFromSqlServer : IProductRepository
    {
        private readonly AppIdentityDbContext _context;

        public ProductRepositoryFromSqlServer(AppIdentityDbContext context)
        {
           
            _context = context;
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            var result = await _context.Products.Where(w => w.UserId == userId).ToListAsync();
            return result;

        }

        public async Task<Product> GetById(string id)
        {
            var result = await _context.Products.FindAsync(id);
            return result;
        }

        public async Task<Product> Save(Product product)
        {
            product.Id = Guid.NewGuid().ToString();

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;

        }

        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

        }
    }
}
