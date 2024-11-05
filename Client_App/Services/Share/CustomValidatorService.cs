using System.ComponentModel.DataAnnotations;

namespace Client_App.Services.Share;

public class CustomValidatorService
{
    public IEnumerable<ValidationResult> Validate(object model, string? propertyName = default)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model);

        if (propertyName is null)
        {
            Validator.TryValidateObject(model, context, validationResults, true);

            return validationResults;
        }

        var property = model.GetType().GetProperty(propertyName);

        if (property is null)
            return validationResults;

        property.GetValue(model);

        context.MemberName = property.Name;

        return validationResults;
    }
}
