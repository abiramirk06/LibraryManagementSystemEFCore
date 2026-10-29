
using LibraryManagementSystemEFCore.Models;

namespace LibraryManagementSystemEFCore.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task<bool> UpdateAsync(int id, Book book);
        Task<bool> DeleteAsync(int id);
    }
}
