using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using static PlasticGui.PlasticTableColumn;


namespace BehaviourTree
{
    /// <summary>
    /// 行为树视图
    /// </summary>
    public class BehaviourTreeEditor : EditorWindow
    {
        BehaviourTreeView treeView;
        InspectorView inspectorView;
        IMGUIContainer blackboardView;

        /// <summary>
        /// BehaviourTree的映射对象，BehaviourTree的改变时，treeObject会同步的改变
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
        /// 双击Project中的BehaviourTree文件时打开BehaviourTreeEditor窗口
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
                    // 更新treeObject
                    treeObject.Update();
                    // 显示tree的blackboard属性
                    EditorGUILayout.PropertyField(blackboardProperty);
                    // 应用并保存更改
                    treeObject.ApplyModifiedProperties();
                }
            };

            treeView.OnNodeSelected = OnNodeSelectionChanged;

            // 确保我们在选中ScriptableObject时打开视图，能直接将选中的内容显示到视图中
            OnSelectionChange();
        }


        // 确保不会双重订阅
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
        /// 这个方法是为了防止程序崩溃：
        /// 在退出PlayMode时，依然显示克隆版本的行为树视图，如果鼠标点击克隆的节点元素，程序会崩溃
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
                    // 在进入playmode时，系统会自动刷新一遍BehaviourTreeEditor窗口，所以不用调用
                    //OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }


        /// <summary>
        /// 选择Hierarchy和Project时会执行该方法
        /// </summary>
        private void OnSelectionChange()
        {
            // 通过选择Project中的ScriptableObject来获取BehaviourTree
            BehaviourTree tree = Selection.activeObject as BehaviourTree;

            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    // 通过选择Hierarchy中的物体来获取BehaviourTree
                    BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                    if (runner)
                    {
                        // 获取选中物体正在运行的BehaviourTree
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
                // 选中的是行为树，更改视图中所展示的内容，变成最新选中的行为树
                // 后半部分的判断是防止，在创建新的行为树时点击该行为树，但unity还没准备好就选择，会报错
                if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                {
                    treeView.PopulateView(tree);
                }
            }

            if (tree)
            {
                // 创建一个要检查的对象
                treeObject = new SerializedObject(tree);
                // 获取BehaviourTree中的blackboard属性
                blackboardProperty = treeObject.FindProperty("blackboard");
            }
        }

        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            inspectorView.UpdateSelection(nodeView);
        }

        /// <summary>
        /// 以每秒10帧的速度更新节点元素的标签
        /// </summary>
        private void OnInspectorUpdate()
        {
            treeView?.UpdateNodeStates();
        }
    }
}