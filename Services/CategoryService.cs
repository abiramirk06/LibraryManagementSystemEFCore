
using LibraryManagementSystemEFCore.Models;
using LibraryManagementSystemEFCore.Repositories;

namespace LibraryManagementSystemEFCore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "Category ID must be greater than zero.");

            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            ValidateCategory(category);

            var categories = await _repository.GetAllAsync();

            if (categories.Any(c =>
                c.CategoryName.Equals(
                    category.CategoryName.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "This category already exists.");
            }

            category.CategoryName = category.CategoryName.Trim();
            category.Description = category.Description?.Trim();

            await _repository.AddAsync(category);
        }

        public async Task<bool> UpdateAsync(
            int id, Category category)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "Category ID must be greater than zero.");

            ValidateCategory(category);

            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return false;

            var categories = await _repository.GetAllAsync();

            if (categories.Any(c =>
                c.CategoryId != id &&
                c.CategoryName.Equals(
                    category.CategoryName.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "This category already exists.");
            }

            existing.CategoryName = category.CategoryName.Trim();
            existing.Description = category.Description?.Trim();

            await _repository.UpdateAsync(existing);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "Category ID must be greater than zero.");

            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return false;

            await _repository.DeleteAsync(category);

            return true;
        }

        private static void ValidateCategory(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);

            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new ArgumentException(
                    "Category name is required.");
            }

            if (category.CategoryName.Trim().Length > 100)
            {
                throw new ArgumentException(
                    "Category name cannot exceed 100 characters.");
            }
        }
    }
}
