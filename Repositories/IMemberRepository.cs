
using LibraryManagementSystemEFCore.Models;

namespace LibraryManagementSystemEFCore.Repositories
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(Member member);
    }
}
