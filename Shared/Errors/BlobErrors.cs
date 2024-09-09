using BaseDomain.Results;

namespace Shared.Errors;

public static class BlobErrors
{
    public static ErrorType ErrorUploadFile()
    {
        return new ErrorType("Blob.Errors", "Upload file to storage wrong");
    }
}
