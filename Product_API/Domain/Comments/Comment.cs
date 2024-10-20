using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Base.Bases;
using Product_API.Domain.Products;

namespace Product_API.Domain.Comments;

public class Comment : Entity
{
    private Comment() { }

    public int CommentId
    {
        get => Id;
        private init => Id = value;
    }
    public int BuyerId { get; private set; }

    [MaxLength(50)]
    public string BuyerEmail { get; private set; } = null!;

    public int Rating { get; private set; }

    [MaxLength(255)]
    public string Title { get; private set; } = null!;

    [MaxLength(510)]
    public string Content { get; private set; } = null!;

    public DateTime CreatedAt { get; private init; } = DateTime.Now;
    public DateTime UpdatedAt { get; private set; } = DateTime.Now;

    [JsonIgnore]
    public Product Product { get; init; } = null!;
    public int? ProductId { get; private set; }

    public static Comment Create(
        int buyerId,
        string buyerEmail,
        int rating,
        string title,
        string content
    )
    {
        if (rating is < 1 or > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5.");
        }

        return new Comment
        {
            BuyerId = buyerId,
            BuyerEmail = buyerEmail,
            Title = title,
            Rating = rating,
            Content = content,
        };
    }

    public void Update(int rating, string title, string content)
    {
        UpdatedAt = DateTime.Now;
        Title = title;
        Rating = rating;
        Content = content;
    }

    public void AssignProduct(int productId)
    {
        if (ProductId is null)
        {
            ProductId = productId;
            return;
        }

        throw new InvalidOperationException("Products can only be assign one time.");
    }
}
