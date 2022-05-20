using NLayer.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsWithCategory();
    }
}
