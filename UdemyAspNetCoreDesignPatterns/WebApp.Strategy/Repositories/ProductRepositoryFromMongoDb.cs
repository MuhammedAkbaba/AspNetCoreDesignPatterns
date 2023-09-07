using MongoDB.Driver;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repositories
{
    /// <summary>
    /// Strategy 2. adım 
    /// </summary>
    public class ProductRepositoryFromMongoDb : IProductRepository
    {
        private readonly IMongoCollection<Product> _productMongoCollection;
        public ProductRepositoryFromMongoDb(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            //MongoDb bağlantı
            var client = new MongoClient(connectionString);
            ///Yeni db oluşturma veya var olan db ismini yazma
            var database = client.GetDatabase("ProductDb");
            ///hangi Product entity sınıfın verilmesi
            _productMongoCollection = database.GetCollection<Product>("Products");

        }

        public async Task Delete(Product product)
        {
            await _productMongoCollection.DeleteOneAsync(x => x.Id == product.Id);
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _productMongoCollection.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await _productMongoCollection.Find(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<Product> Save(Product product)
        {
           /// product.Id = Guid.NewGuid().ToString(); Guid değeri mongodb kendisi üretiyor

            await _productMongoCollection.InsertOneAsync(product);

            return product;
        }

        public async Task Update(Product product)
        {
            await _productMongoCollection.FindOneAndReplaceAsync(x => x.Id == product.Id, product);
        }
    }
}
