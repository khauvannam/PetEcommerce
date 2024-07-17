using BaseDomain.Results;

namespace Shared.Errors;

public static class BlobErrors
{
    public static ErrorType ErrorUploadFile() => new("Blob.Errors", "Upload file to storage wrong");
}
