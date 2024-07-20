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
        /// ˫��Project�е�BehaviourTree�ļ�ʱ��BehaviourTreeEditor����
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
                // ��벿�ֵ��ж��Ƿ�ֹ���ڴ����µ���Ϊ��ʱ�������Ϊ������unity��û׼���þ�ѡ�񣬻ᱨ��
                if (controller && AssetDatabase.CanOpenAssetInEditor(controller.GetInstanceID()))
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
    }
}