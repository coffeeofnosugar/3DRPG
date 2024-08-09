#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;
using UnityEditor;

[assembly: RegisterValidator(typeof(CharacterStatsValidator))]

public class CharacterStatsValidator : RootObjectValidator<CharacterStats>
{
    protected override void Validate(ValidationResult result)
    {
        if (string.IsNullOrEmpty(this.Value.MonsterName))
            result.AddWarning("Please provide a name for the item");
        if (this.Value.MaxHealth <= 0)
            result.AddWarning("The MaxHealth has to over zero");
    }
}
#endif
