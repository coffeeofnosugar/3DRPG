using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public abstract class CompositeNode : Node
    {
        public Node[] children = new Node[4];
    }
}