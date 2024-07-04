using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 复合节点
/// 类似控制流，for  if等
/// 多个子节点
/// </summary>
public abstract class CompositeNode : Node
{
    [HideInInspector] public List<Node> children = new List<Node>();
}