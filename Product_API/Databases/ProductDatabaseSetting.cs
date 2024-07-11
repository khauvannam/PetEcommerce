namespace Product_API.Databases;

public class ProductDatabaseSetting
{
    public required string ConnectionString { get; init; } = null!;
    public required int DatabaseIndex { get; init; }
}
