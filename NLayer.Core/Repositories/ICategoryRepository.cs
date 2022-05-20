using NLayer.Core.Models;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetSingleCategoryByIdWithProductAsync(int id);
    }
}
