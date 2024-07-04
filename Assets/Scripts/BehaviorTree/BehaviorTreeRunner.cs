using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϊ��������
/// </summary>
public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;

    private void Start()
    {
        tree = tree.Clone();
    }

    private void Update()
    {
        tree.Update();
    }
}