using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player.PlayerController
{
    public class PlayerControllerEditor : EditorWindow
    {
        private PlayerControllerView controllerView;
        private InspectorView inspectorView;
        private IMGUIContainer blackboardView;

        private SerializedObject controllerObject;
        private SerializedProperty blackboardProperty;
        
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Tools/PlayerControllerEditor")]
        public static void OpenWindow()
        {
            PlayerControllerEditor wnd = GetWindow<PlayerControllerEditor>();
            wnd.titleContent = new GUIContent("PlayerControllerEditor");
        }
        
        /// <summary>
        /// 双击Project中的BehaviourTree文件时打开BehaviourTreeEditor窗口
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId)
        {
            if (Selection.activeObject is PlayerController)
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

            m_VisualTreeAsset.CloneTree(root);
            controllerView = root.Q<PlayerControllerView>();
            inspectorView = root.Q<InspectorView>();
            blackboardView = root.Q<IMGUIContainer>();

            controllerView.OnNodeSelected = OnNodeSelectionChanged;
            
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
                    // 在进入PlayMode时，系统会自动刷新一遍BehaviourTreeEditor窗口，所以不用调用
                    //OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        private void OnSelectionChange()
        {
            PlayerController controller = Selection.activeObject as PlayerController;

            if (!controller)
            {
                if (Selection.activeGameObject)
                {
                    PlayerControllerRunner runner = Selection.activeGameObject.GetComponent<PlayerControllerRunner>();
                    if (runner)
                    {
                        controller = runner.controller;
                    }
                }
            }

            if (Application.isPlaying)
            {
                if (controller)
                {
                    controllerView.PopulateView(controller);
                }
            }
            else
            {
                // 后半部分的判断是防止，在创建新的行为树时点击该行为树，但unity还没准备好就选择，会报错
                if (controller && controllerView != null && AssetDatabase.CanOpenAssetInEditor(controller.GetInstanceID()))
                {
                    controllerView.PopulateView(controller);
                }
            }

            if (controller)
            {
                controllerObject = new SerializedObject(controller);
                blackboardProperty = controllerObject.FindProperty("blackboard");
            }
        }

        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            inspectorView.UpdateSelection(nodeView);
        }

        // 舍弃该方法，直接在进入离开节点使改变标签
        // private void OnInspectorUpdate()
        // {
        //     controllerView?.UpdateNodeStates();
        // }
    }
}