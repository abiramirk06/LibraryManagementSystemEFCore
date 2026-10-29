
using LibraryManagementSystemEFCore.Data;
using LibraryManagementSystemEFCore.Models;
using LibraryManagementSystemEFCore.Repositories;
using LibraryManagementSystemEFCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        @"Server=(localdb)\MSSQLLocalDB;Database=LibraryManagementSystemEFCoreDB;Trusted_Connection=True;TrustServerCertificate=True"));

services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<ICategoryService, CategoryService>();

using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();

var categoryService =
    scope.ServiceProvider.GetRequiredService<ICategoryService>();

while (true)
{
    Console.WriteLine("\n=== Category Management ===");
    Console.WriteLine("1. Add Category");
    Console.WriteLine("2. View Categories");
    Console.WriteLine("3. Search Category by ID");
    Console.WriteLine("4. Update Category");
    Console.WriteLine("5. Delete Category");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                Console.Write("Category name: ");
                string name = Console.ReadLine() ?? "";

                Console.Write("Description: ");
                string? description = Console.ReadLine();

                await categoryService.AddAsync(new Category
                {
                    CategoryName = name,
                    Description = description
                });

                Console.WriteLine("Category added successfully.");
                break;

            case "2":
                var categories = await categoryService.GetAllAsync();

                foreach (var category in categories)
                {
                    Console.WriteLine(
                        $"{category.CategoryId} | " +
                        $"{category.CategoryName} | " +
                        $"{category.Description}");
                }

                if (categories.Count == 0)
                    Console.WriteLine("No categories found.");

                break;

            case "3":
                Console.Write("Enter category ID: ");
                int searchId = int.Parse(Console.ReadLine()!);

                var found = await categoryService.GetByIdAsync(searchId);

                Console.WriteLine(found == null
                    ? "Category not found."
                    : $"{found.CategoryId} | {found.CategoryName} | {found.Description}");
                break;

            case "4":
                Console.Write("Enter category ID: ");
                int updateId = int.Parse(Console.ReadLine()!);

                Console.Write("New category name: ");
                string newName = Console.ReadLine() ?? "";

                Console.Write("New description: ");
                string? newDescription = Console.ReadLine();

                bool updated = await categoryService.UpdateAsync(
                    updateId,
                    new Category
                    {
                        CategoryName = newName,
                        Description = newDescription
                    });

                Console.WriteLine(updated
                    ? "Category updated successfully."
                    : "Category not found.");
                break;

            case "5":
                Console.Write("Enter category ID: ");
                int deleteId = int.Parse(Console.ReadLine()!);

                bool deleted = await categoryService.DeleteAsync(deleteId);

                Console.WriteLine(deleted
                    ? "Category deleted successfully."
                    : "Category not found.");
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
