using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace BehaviourTree
{
    /// <summary>
    /// ��Ϊ����ͼ
    /// </summary>
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

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTree/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviourTreeView>();
            inspectorView = root.Q<InspectorView>();
            treeView.OnNodeSelected = OnNodeSelectionChanged;

            // ȷ��������ѡ��ScriptableObjectʱ����ͼ����ֱ�ӽ�ѡ�е�������ʾ����ͼ��
            OnSelectionChange();
        }

        /// <summary>
        /// ѡ��Hierarchy��Projectʱ��ִ�и÷���
        /// </summary>
        private void OnSelectionChange()
        {
            BehaviourTree tree = Selection.activeObject as BehaviourTree;
            // ѡ�е�����Ϊ����������ͼ����չʾ�����ݣ��������ѡ�е���Ϊ��
            // ��벿�ֵ��ж��Ƿ�ֹ���ڴ����µ���Ϊ������unity��û׼���þ�ѡ�񣬻ᱨ��
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView.PopulateView(tree);
            }
        }

        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            inspectorView.UpdateSelection(nodeView);
        }
    }
}