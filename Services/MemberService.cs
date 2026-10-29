
using LibraryManagementSystemEFCore.Models;
using LibraryManagementSystemEFCore.Repositories;

namespace LibraryManagementSystemEFCore.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Member>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Member?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Member ID must be greater than zero.");

            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Member member)
        {
            ValidateMember(member);

            var members = await _repository.GetAllAsync();
            string email = member.Email.Trim();

            if (members.Any(m => m.Email.Equals(
                email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException(
                    "A member with this email already exists.");

            member.FullName = member.FullName.Trim();
            member.Email = email;
            member.PhoneNumber = member.PhoneNumber?.Trim();
            member.MembershipDate = DateTime.Today;

            await _repository.AddAsync(member);
        }

        public async Task<bool> UpdateAsync(int id, Member member)
        {
            if (id <= 0)
                throw new ArgumentException("Member ID must be greater than zero.");

            ValidateMember(member);

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            string email = member.Email.Trim();
            var members = await _repository.GetAllAsync();

            if (members.Any(m => m.MemberId != id &&
                m.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException(
                    "A member with this email already exists.");

            existing.FullName = member.FullName.Trim();
            existing.Email = email;
            existing.PhoneNumber = member.PhoneNumber?.Trim();
            existing.IsActive = member.IsActive;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Member ID must be greater than zero.");

            var member = await _repository.GetByIdAsync(id);
            if (member == null)
                return false;

            await _repository.DeleteAsync(member);
            return true;
        }

        private static void ValidateMember(Member member)
        {
            ArgumentNullException.ThrowIfNull(member);

            if (string.IsNullOrWhiteSpace(member.FullName))
                throw new ArgumentException("Full name is required.");

            if (member.FullName.Trim().Length > 100)
                throw new ArgumentException("Full name cannot exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(member.Email))
                throw new ArgumentException("Email is required.");

            if (member.Email.Trim().Length > 150 ||
                !System.Net.Mail.MailAddress.TryCreate(
                    member.Email.Trim(), out var address) ||
                !address.Address.Equals(
                    member.Email.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Enter a valid email address.");

            if (member.PhoneNumber?.Length > 15)
                throw new ArgumentException("Phone number cannot exceed 15 characters.");
        }
    }
}
