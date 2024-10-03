using Product_API.Databases;

namespace Product_API.Services;

public class ProductService(ProductDbContext context)
{
    public bool CheckDuplicateProductName(string productName)
    {
        var productsNameList = context.Products.Select(p => p.Name).ToList();
        return productsNameList.Contains(productName);
    }
}
