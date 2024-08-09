#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;
using UnityEditor;

[assembly: RegisterValidationRule(typeof(EmptyStringValidator), Name = "EmptyStringValidator", Description = "Some description text.")]

public class EmptyStringValidator : ValueValidator<string>
{
    // Introduce serialized fields here to make your validator
    // configurable from the validator window under rules.
    public int SerializedConfig;
    public ValidatorSeverity Severity;

    protected override void Validate(ValidationResult result)
    {
        if (string.IsNullOrEmpty(this.Value))
        {
            result.Add(Severity, "This string is empty! Are you sure that's correct?");
        }
    }
}
#endif
