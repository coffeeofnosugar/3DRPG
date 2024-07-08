using UnityEditor;
using UnityEngine.UIElements;


namespace BehaviourTree
{
    /// <summary>
    /// 行为树视图左边区域
    /// </summary>
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private Editor editor;

        public InspectorView()
        {

        }

        /// <summary>
        /// 显示当前选中的节点元素的Inspector内容
        /// </summary>
        /// <param name="nodeView"></param>
        internal void UpdateSelection(NodeView nodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = Editor.CreateEditor(nodeView.node);
            // 创建一个容器，并将节点的Inspector窗口上的内容显示到左侧窗口
            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor && editor.target)  // 如果选择的节点元素还存在
                {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
        }
    }
}