namespace Product_API.DTOs.Categories;

public record UpdateCategoryRequest(string CategoryName, string Description, IFormFile? File);
