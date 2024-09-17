namespace Product_API.Domains.Comments;

public class CommentBuyerId
{
    private CommentBuyerId() { }

    public Guid UserId { get; private init; }
    public Comment Comment { get; init; } = null!;
    public int CommentId { get; init; }

    public static CommentBuyerId Create(Guid userId)
    {
        return new() { UserId = userId };
    }
}
