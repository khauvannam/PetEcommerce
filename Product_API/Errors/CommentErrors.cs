using BaseDomain.Results;

namespace Product_API.Errors;

public static class CommentErrors
{
    public static ErrorType NotFound =>
        new("Comment.NotFound", "Your Comment Not Found, Try contact with supporter");
}
