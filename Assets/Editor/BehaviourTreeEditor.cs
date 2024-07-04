using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree
{
    public class BehaviourTreeEditor : EditorWindow
    {
        BehaviourTreeView treeView;
        InspectorView inspectorView;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("BehaviourTreeEditor/Editor ...")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            //// Instantiate UXML
            //VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
            //root.Add(labelFromUXML);
            m_VisualTreeAsset.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviourTreeView>();
            inspectorView = root.Q<InspectorView>();
        }

        //private void OnSelectionChange()
        //{
        //    BehaviorTree tree = Selection.activeObject as BehaviorTree;

        //    if (tree)
        //    {
        //        treeView
        //    }
        //}
    }
}