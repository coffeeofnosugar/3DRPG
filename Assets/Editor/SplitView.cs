using UnityEngine.UIElements;

/// <summary>
/// 将窗口左右分成两半
/// </summary>
public class SplitView : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
}
