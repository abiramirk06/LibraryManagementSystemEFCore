
using LibraryManagementSystemEFCore.Models;
using LibraryManagementSystemEFCore.Repositories;

namespace LibraryManagementSystemEFCore.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;

        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Author ID must be greater than zero.");

            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Author author)
        {
            ValidateAuthor(author);

            var authors = await _repository.GetAllAsync();

            if (authors.Any(a => a.AuthorName.Equals(
                author.AuthorName.Trim(),
                StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("This author already exists.");
            }

            author.AuthorName = author.AuthorName.Trim();
            author.Biography = author.Biography?.Trim();
            author.Country = author.Country?.Trim();

            await _repository.AddAsync(author);
        }

        public async Task<bool> UpdateAsync(int id, Author author)
        {
            if (id <= 0)
                throw new ArgumentException("Author ID must be greater than zero.");

            ValidateAuthor(author);

            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return false;

            var authors = await _repository.GetAllAsync();

            if (authors.Any(a => a.AuthorId != id &&
                a.AuthorName.Equals(
                    author.AuthorName.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("This author already exists.");
            }

            existing.AuthorName = author.AuthorName.Trim();
            existing.Biography = author.Biography?.Trim();
            existing.Country = author.Country?.Trim();

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Author ID must be greater than zero.");

            var author = await _repository.GetByIdAsync(id);

            if (author == null)
                return false;

            await _repository.DeleteAsync(author);
            return true;
        }

        private static void ValidateAuthor(Author author)
        {
            ArgumentNullException.ThrowIfNull(author);

            if (string.IsNullOrWhiteSpace(author.AuthorName))
                throw new ArgumentException("Author name is required.");

            if (author.AuthorName.Trim().Length > 100)
                throw new ArgumentException("Author name cannot exceed 100 characters.");
        }
    }
}
