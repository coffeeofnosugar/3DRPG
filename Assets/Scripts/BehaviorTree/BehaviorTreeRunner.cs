using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÐÐÎªÊ÷Æô¶¯Æ÷
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