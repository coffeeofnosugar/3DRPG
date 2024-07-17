using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public abstract class WhetherNode : Node
    {
        public Node[] children = new Node[2];
    }
}