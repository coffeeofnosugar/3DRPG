using UnityEngine;

/// <summary>
/// 需要公共描述字段的ScriptableObjects的基类。
/// </summary>
public class DescriptionBaseSO : ScriptableObject
{
    [TextArea] public string description;
}
