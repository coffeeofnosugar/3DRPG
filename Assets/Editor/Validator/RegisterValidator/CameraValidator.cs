#if UNITY_EDITOR
using Player;
using Sirenix.OdinInspector.Editor.Validation;

[assembly: RegisterValidator(typeof(CameraValidator))]

public class CameraValidator : ValueValidator<PlayerCamera>
{
    protected override void Validate(ValidationResult result)
    {
        if (Value.player == null)
            result.AddError("Camera need player");
    }
}
#endif