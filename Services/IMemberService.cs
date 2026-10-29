
using LibraryManagementSystemEFCore.Models;

namespace LibraryManagementSystemEFCore.Services
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task AddAsync(Member member);
        Task<bool> UpdateAsync(int id, Member member);
        Task<bool> DeleteAsync(int id);
    }
}
