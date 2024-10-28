namespace Client_App.DTOs.Products.Requests;

public class ProductsSearchFilterRequest
{
    public string? SearchText = default;
    public int Limit = 5;
    public int Offset = default;
    public decimal MinPrice = decimal.Zero;
    public decimal MaxPrice = 10000M;
    public string? FilerBy = default;
    public bool Available = true;
    public bool IsDesc = default;
}
