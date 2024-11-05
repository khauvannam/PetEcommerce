using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client_App.Components.Share.CustomInputs;

public partial class CustomInputText : ComponentBase
{
    [Parameter]
    public string? PlaceHolder { get; set; }

    [Parameter]
    public string? ClassCss { get; set; }

    [Parameter]
    public string Value { get; set; } = default!;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public EventCallback OnChange { get; set; }

    [Parameter]
    public string? PropertyToValidate { get; set; }

    [CascadingParameter]
    private EditContext? EditContext { get; set; }

    private ValidationMessageStore? ValidationMessageStore { get; set; }
    private FieldIdentifier FieldIdentifier { get; set; }
    private object Model { get; set; } = null!;

    private void OnInput(string? value)
    {
        if (EqualityComparer<string?>.Default.Equals(Value, value))
            return;

        Value = value!;
        ValueChanged.InvokeAsync(Value);

        if (OnChange.HasDelegate)
            OnChange.InvokeAsync();

        ValidateProperty();
    }

    private void ValidateProperty()
    {
        if (string.IsNullOrEmpty(PropertyToValidate))
            return;

        var validationResults = CustomValidatorService.Validate(Model, PropertyToValidate).ToList();

        var fieldIdentifier = new FieldIdentifier(Model, PropertyToValidate);
        EditContext?.NotifyFieldChanged(fieldIdentifier);

        if (validationResults.Any())
            ValidationMessageStore?.Add(
                fieldIdentifier,
                validationResults.Select(v => v.ErrorMessage)!
            );
        else
            ValidationMessageStore?.Clear(fieldIdentifier);
    }

    protected override void OnInitialized()
    {
        if (EditContext is null)
            return;

        Model = EditContext.Model;

        ValidationMessageStore = new ValidationMessageStore(EditContext);
    }
}
