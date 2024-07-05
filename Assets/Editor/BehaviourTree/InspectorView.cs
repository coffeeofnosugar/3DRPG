using UnityEditor;
using UnityEngine.UIElements;

/// <summary>
/// ��Ϊ����ͼ�������
/// </summary>
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    private Editor editor;

    public InspectorView()
    {

    }

    /// <summary>
    /// ��ʾ��ǰѡ�еĽڵ�Ԫ�ص�Inspector����
    /// </summary>
    /// <param name="nodeView"></param>
    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        // ����һ�������������ڵ��Inspector�����ϵ�������ʾ����ര��
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);
    }
}