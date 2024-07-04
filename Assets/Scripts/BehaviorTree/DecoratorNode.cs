using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装饰器节点
/// 
/// 一个子节点
/// </summary>
public abstract class DecoratorNode : Node
{
    public Node child;
}