using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ͻڵ�
/// ���ƿ�������for  if��
/// ����ӽڵ�
/// </summary>
public abstract class CompositeNode : Node
{
    [HideInInspector] public List<Node> children = new List<Node>();
}