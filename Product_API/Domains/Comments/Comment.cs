using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;
using Product_API.Domains.Products;

namespace Product_API.Domains.Comments;

public class Comment
{
    private Comment() { }

    public int CommentId { get; init; }
    public Guid BuyerId { get; private set; }
    public int Rating { get; private set; }

    [MaxLength(255)]
    public string Content { get; private set; } = null!;

    public DateTime CreatedAt { get; private init; } = DateTime.Now;
    public DateTime UpdatedAt { get; private set; } = DateTime.Now;
    public Product Product { get; init; } = null!;
    public Guid ProductId { get; init; }

    public static Comment Create(Guid buyerId, int rating, string content)
    {
        if (rating is < 1 or > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5.");
        }

        return new Comment
        {
            BuyerId = buyerId,
            Rating = rating,
            Content = content,
        };
    }

    public void Update(int rating, string content)
    {
        UpdatedAt = DateTime.Now;
        Rating = rating;
        Content = content;
    }
}
