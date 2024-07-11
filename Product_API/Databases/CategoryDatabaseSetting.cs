namespace Product_API.Databases;

public class CategoryDatabaseSetting
{
    public required string ConnectionString { get; init; } = null!;
    public required int DatabaseIndex { get; init; }
}
