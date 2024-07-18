using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    [CreateAssetMenu()]
    public class PlayerController : ScriptableObject
    {
        public Node rootNode;
        public Node.State controllerState = Node.State.Running;
        public List<Node> nodes = new List<Node>();

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                controllerState = rootNode.Update();
            }
            return controllerState;
        }
    }
}