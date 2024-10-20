using Client_App.DTOs.Share;

namespace Client_App.Services.Share;

public sealed class ErrorService
{
    private ErrorType _errorMessage = Errors.None;
    public string? Url { get; private set; }

    public ErrorType ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value ?? throw new ArgumentNullException(nameof(ErrorMessage));
            NotifyErrorHappened();
        }
    }

    private event Action? OnErrorHappened;

    public void SubscribeEvent(Action onErrorHappened)
    {
        OnErrorHappened += onErrorHappened;
    }

    public void UnsubscribeEvent(Action onErrorHappened)
    {
        OnErrorHappened -= onErrorHappened;
    }

    public ErrorService SetErrorMessage(ErrorType errorMessage)
    {
        ErrorMessage = errorMessage;
        return this;
    }

    public void NavigateTo(string? url = "/")
    {
        Url = url;
    }

    public void ClearErrorMessage()
    {
        ErrorMessage = Errors.None;
        NotifyErrorHappened();
    }

    private void NotifyErrorHappened()
    {
        OnErrorHappened?.Invoke();
    }
}
