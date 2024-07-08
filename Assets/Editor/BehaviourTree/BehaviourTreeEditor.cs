using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using static PlasticGui.PlasticTableColumn;


namespace BehaviourTree
{
    /// <summary>
    /// ��Ϊ����ͼ
    /// </summary>
    public class BehaviourTreeEditor : EditorWindow
    {
        BehaviourTreeView treeView;
        InspectorView inspectorView;
        IMGUIContainer blackboardView;

        /// <summary>
        /// BehaviourTree��ӳ�����BehaviourTree�ĸı�ʱ��treeObject��ͬ���ĸı�
        /// </summary>
        SerializedObject treeObject;
        SerializedProperty blackboardProperty;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Tools/BehaviourTreeEditor")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        }

        /// <summary>
        /// ˫��Project�е�BehaviourTree�ļ�ʱ��BehaviourTreeEditor����
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                OpenWindow();
                return true;
            }
            return false;
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
            blackboardView = root.Q<IMGUIContainer>();
            blackboardView.onGUIHandler = () =>
            {
                if (treeObject != null && treeObject.targetObject != null)
                {
                    // ����treeObject
                    treeObject.Update();
                    // ��ʾtree��blackboard����
                    EditorGUILayout.PropertyField(blackboardProperty);
                    // Ӧ�ò��������
                    treeObject.ApplyModifiedProperties();
                }
            };

            treeView.OnNodeSelected = OnNodeSelectionChanged;

            // ȷ��������ѡ��ScriptableObjectʱ����ͼ����ֱ�ӽ�ѡ�е�������ʾ����ͼ��
            OnSelectionChange();
        }


        // ȷ������˫�ض���
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        /// <summary>
        /// ���������Ϊ�˷�ֹ���������
        /// ���˳�PlayModeʱ����Ȼ��ʾ��¡�汾����Ϊ����ͼ������������¡�Ľڵ�Ԫ�أ���������
        /// </summary>
        /// <param name="change"></param>
        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    // �ڽ���playmodeʱ��ϵͳ���Զ�ˢ��һ��BehaviourTreeEditor���ڣ����Բ��õ���
                    //OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }


        /// <summary>
        /// ѡ��Hierarchy��Projectʱ��ִ�и÷���
        /// </summary>
        private void OnSelectionChange()
        {
            // ͨ��ѡ��Project�е�ScriptableObject����ȡBehaviourTree
            BehaviourTree tree = Selection.activeObject as BehaviourTree;

            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    // ͨ��ѡ��Hierarchy�е���������ȡBehaviourTree
                    BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                    if (runner)
                    {
                        // ��ȡѡ�������������е�BehaviourTree
                        tree = runner.tree;
                    }
                }
            }

            if (Application.isPlaying)
            {
                if (tree)
                {
                    treeView.PopulateView(tree);
                }
            }
            else
            {
                // ѡ�е�����Ϊ����������ͼ����չʾ�����ݣ��������ѡ�е���Ϊ��
                // ��벿�ֵ��ж��Ƿ�ֹ���ڴ����µ���Ϊ��ʱ�������Ϊ������unity��û׼���þ�ѡ�񣬻ᱨ��
                if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                {
                    treeView.PopulateView(tree);
                }
            }

            if (tree)
            {
                // ����һ��Ҫ���Ķ���
                treeObject = new SerializedObject(tree);
                // ��ȡBehaviourTree�е�blackboard����
                blackboardProperty = treeObject.FindProperty("blackboard");
            }
        }

        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            inspectorView.UpdateSelection(nodeView);
        }

        /// <summary>
        /// ��ÿ��10֡���ٶȸ��½ڵ�Ԫ�صı�ǩ
        /// </summary>
        private void OnInspectorUpdate()
        {
            treeView?.UpdateNodeStates();
        }
    }
}