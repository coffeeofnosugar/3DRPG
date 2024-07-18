using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player.PlayerController
{
    public class PlayerControllerEditor : EditorWindow
    {
        private PlayerControllerView playerControllerView;
        private InspectorView inspectorView;
        private IMGUIContainer blackboardView;

        private SerializedObject playerControllerObject;
        
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
            playerControllerView = root.Q<PlayerControllerView>();
            inspectorView = root.Q<InspectorView>();
            blackboardView = root.Q<IMGUIContainer>();

            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            PlayerController playerController = Selection.activeObject as PlayerController;

            // ��벿�ֵ��ж��Ƿ�ֹ���ڴ����µ���Ϊ��ʱ�������Ϊ������unity��û׼���þ�ѡ�񣬻ᱨ��
            if (playerController && AssetDatabase.CanOpenAssetInEditor(playerController.GetInstanceID()))
            {
                playerControllerView.PopulateView(playerController);
                playerControllerObject = new SerializedObject(playerController);
            }
        }
    }
}