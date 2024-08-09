#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;
using UnityEditor;

[assembly: RegisterValidator(typeof(NeedsComponentAttributeValidator))]

public class NeedsComponentAttributeValidator : AttributeValidator<NeedsComponentAttribute, GameObject>
{
    protected override void Validate(ValidationResult result)
    {
        if (this.Value == null)
            return;
        if (this.Value.GetComponent(this.Attribute.type) == null)
            result.AddError($"This gameObject needs {this.Attribute.type} component.");
        //var attr = this.Attribute;
        //var val = this.Value;

        //if (val has something wrong with it)
        //{
        //    result.AddError("Something is wrong");
        //}
    }
}

// Alternative version that does not target a specific type of value (above must be deleted or commented out for the below to work)

//[assembly: RegisterValidator(typeof(NeedsComponentAttributeValidator))]

//public class NeedsComponentAttributeValidator : AttributeValidator<NeedsComponentAttribute>
//{
//    protected override void Validate(ValidationResult result)
//    {
//        //var attr = this.Attribute;
//
//        //if (if something is wrong)
//        //{
//        //    result.AddError("Something is wrong");
//        //}
//    }
//}
#endif
