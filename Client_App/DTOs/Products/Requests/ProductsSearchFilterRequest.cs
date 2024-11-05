namespace Client_App.DTOs.Products.Requests;

public class ProductsSearchFilterRequest
{
    public bool Available = true;
    public string? FilerBy = default;
    public bool IsDesc = default;
    public int Limit = 5;
    public decimal MaxPrice = 10000M;
    public decimal MinPrice = decimal.Zero;
    public int Offset = default;
    public string? SearchText = default;
}
