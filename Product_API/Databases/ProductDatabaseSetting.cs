namespace Product_API.Databases;

public class ProductDatabaseSetting
{
    public required string ConnectionString { get; set; } = null!;
    public required string DatabaseName { get; set; } = null!;
    public required string CollectionName { get; set; } = null!;
}
