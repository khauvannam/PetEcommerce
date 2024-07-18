namespace Product_API.Domains.Categories;

public record UpdateCategoryRequest(string CategoryName, string Description, IFormFile? File);
