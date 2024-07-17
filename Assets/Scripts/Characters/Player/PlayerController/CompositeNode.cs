using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();
    }
}