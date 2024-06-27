namespace Product_API.Databases;

public class CategoryDatabaseSetting
{
    public required string ConnectionString { get; init; } = null!;
    public required string DatabaseName { get; init; } = null!;
    public required string CollectionName { get; init; } = null!;
}
