using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player.PlayerController
{
    public class PlayerControllerEditor : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Tools/PlayerControllerEditor")]
        public static void OpenWindow()
        {
            PlayerControllerEditor wnd = GetWindow<PlayerControllerEditor>();
            wnd.titleContent = new GUIContent("PlayerControllerEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            m_VisualTreeAsset.CloneTree(root);
        }
    }
}