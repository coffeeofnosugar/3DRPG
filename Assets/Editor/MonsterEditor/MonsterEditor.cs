using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterEditor
{
    public class MonsterEditor : EditorWindow
    {
        Button button;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Tools/MonsterEditor")]
        public static void OpenWindow()
        {
            MonsterEditor wnd = GetWindow<MonsterEditor>();
            wnd.titleContent = new GUIContent("MonsterEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            m_VisualTreeAsset.CloneTree(root);

            button = root.Q<Button>("NameButton");
            Debug.Log(button.text);
        }
    }
}
