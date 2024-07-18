using BehaviourTree;
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
    /// <param name="node"></param>
    internal void UpdateSelection(UnityEditor.Experimental.GraphView.Node node)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);

        if (node is BehaviourTree.NodeView nodeView)
            editor = Editor.CreateEditor(nodeView.node);
        
        // ����һ�������������ڵ��Inspector�����ϵ�������ʾ����ര��
        IMGUIContainer container = new IMGUIContainer(() => {
            if (editor && editor.target)  // ���ѡ��Ľڵ�Ԫ�ػ�����
            {
                editor.OnInspectorGUI();
            }
        });
        Add(container);
    }
}