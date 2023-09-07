using MongoDB.Bson;
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
            var database = client.GetDatabase("bymdakbaba");
            ///hangi Product entity sınıfın verilmesi
            _productMongoCollection = database.GetCollection<Product>("Products");

            
            //const string connectionUri = "mongodb+srv://bymdakbaba:qDDReAtn6IzTHfOG@cluster0.ufheob5.mongodb.net/?retryWrites=true&w=majority";
            //var settings = MongoClientSettings.FromConnectionString(connectionUri);
            //// Set the ServerApi field of the settings object to Stable API version 1
            //settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            //// Create a new client and connect to the server
            //var client2 = new MongoClient(settings);
            //// Send a ping to confirm a successful connection
            //try
            //{
            //    var result = client2.GetDatabase("bymdakbaba").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            //    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            //}
            //catch (Exception ex)
            //{

            //}

            
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
