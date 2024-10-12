namespace Client_App.Domains.Comments.Responses;

public class Comment
{
    public int CommentId { get; init; }
    public Guid BuyerId { get; init; }
    public required string BuyerEmail { get; init; }
    public int Rating { get; init; }

    public required string Title { get; init; }

    public required string Content { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid? ProductId { get; init; }
}
