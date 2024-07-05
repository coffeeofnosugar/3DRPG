using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÐÐÎªÊ÷Ö´ÐÐÆ÷
/// </summary>
public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    private void Start()
    {
        tree = tree.Clone();
    }

    private void Update()
    {
        tree.Update();
    }
}