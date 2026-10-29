
using LibraryManagementSystemEFCore.Models;

namespace LibraryManagementSystemEFCore.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync();

        Task<Author?> GetByIdAsync(int id);

        Task AddAsync(Author author);

        Task<bool> UpdateAsync(int id, Author author);

        Task<bool> DeleteAsync(int id);
    }
}
