namespace Client_App.DTOs.Share;

public record Pagination<T>(List<T> Data, int TotalItems)
{
    public List<T> Data { get; } = Data;
    public int TotalItems { get; } = TotalItems;
}
