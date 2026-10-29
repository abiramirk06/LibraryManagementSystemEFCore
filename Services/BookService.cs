
using LibraryManagementSystemEFCore.Models;
using LibraryManagementSystemEFCore.Repositories;

namespace LibraryManagementSystemEFCore.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(
            IBookRepository repository,
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
        }

        public async Task<List<Book>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Book?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Book ID must be greater than zero.");

            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Book book)
        {
            ValidateBook(book);
            await ValidateReferencesAsync(book);

            book.Title = book.Title.Trim();
            book.ISBN = book.ISBN?.Trim();
            await _repository.AddAsync(book);
        }

        public async Task<bool> UpdateAsync(int id, Book book)
        {
            if (id <= 0)
                throw new ArgumentException("Book ID must be greater than zero.");

            ValidateBook(book);
            await ValidateReferencesAsync(book);

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            existing.Title = book.Title.Trim();
            existing.ISBN = book.ISBN?.Trim();
            existing.PublicationYear = book.PublicationYear;
            existing.Price = book.Price;
            existing.AvailableCopies = book.AvailableCopies;
            existing.CategoryId = book.CategoryId;
            existing.AuthorId = book.AuthorId;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Book ID must be greater than zero.");

            var book = await _repository.GetByIdAsync(id);
            if (book == null)
                return false;

            await _repository.DeleteAsync(book);
            return true;
        }

        private async Task ValidateReferencesAsync(Book book)
        {
            if (await _categoryRepository.GetByIdAsync(book.CategoryId) == null)
                throw new ArgumentException("The selected category does not exist.");

            if (await _authorRepository.GetByIdAsync(book.AuthorId) == null)
                throw new ArgumentException("The selected author does not exist.");
        }

        private static void ValidateBook(Book book)
        {
            ArgumentNullException.ThrowIfNull(book);

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title is required.");

            if (book.Title.Trim().Length > 200)
                throw new ArgumentException("Book title cannot exceed 200 characters.");

            if (book.ISBN?.Length > 20)
                throw new ArgumentException("ISBN cannot exceed 20 characters.");

            if (book.PublicationYear < 0 ||
                book.PublicationYear > DateTime.Now.Year)
                throw new ArgumentException("Publication year is invalid.");

            if (book.Price < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (book.AvailableCopies < 0)
                throw new ArgumentException("Available copies cannot be negative.");

            if (book.CategoryId <= 0 || book.AuthorId <= 0)
                throw new ArgumentException("Valid category and author IDs are required.");
        }
    }
}
