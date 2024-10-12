namespace Client_App.Domains.Share;

public record Pagination<T>(List<T> Data, int TotalItems)
{
    public List<T> Data { get; } = Data;
    public int TotalItems { get; } = TotalItems;
}
