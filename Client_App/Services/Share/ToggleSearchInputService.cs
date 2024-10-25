namespace Client_App.Services.Share;

public class ToggleSearchInputService
{
    private bool _isHidden = true;

    public bool IsHidden
    {
        get => _isHidden;
        private set
        {
            _isHidden = value;
            NotifyOnClickHappened();
        }
    }

    private event Action? OnClickHappened;

    public void SetIsHidden(bool isHidden)
    {
        IsHidden = isHidden;
    }

    public void SubscribeEvents(Action onClickHappened)
    {
        OnClickHappened += onClickHappened;
    }

    public void UnsubscribeEvents(Action onClickHappened)
    {
        OnClickHappened -= onClickHappened;
    }

    private void NotifyOnClickHappened()
    {
        OnClickHappened?.Invoke();
    }
}
