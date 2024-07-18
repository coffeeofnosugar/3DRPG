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
            // 可缩放
            this.AddManipulator(new ContentZoomer());
            // 可拖动
            this.AddManipulator(new ContentDragger());
            // 可选择
            this.AddManipulator(new SelectionDragger());
            // 可框中选择
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/GridBackground.uss");
            styleSheets.Add(styleSheet);
        }
    }
}