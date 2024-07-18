using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace Player.PlayerController
{
    public class PlayerControllerView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<PlayerControllerView, GraphView.UxmlTraits> { }

        public PlayerControllerView()
        {
            Insert(0, new GridBackground());
            // ������
            this.AddManipulator(new ContentZoomer());
            // ���϶�
            this.AddManipulator(new ContentDragger());
            // ��ѡ��
            this.AddManipulator(new SelectionDragger());
            // �ɿ���ѡ��
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/GridBackground.uss");
            styleSheets.Add(styleSheet);
        }
    }
}