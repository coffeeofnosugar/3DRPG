using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑板，存储一些行为树会用到的变量
/// </summary>
[System.Serializable]
public class Blackboard
{
    public Vector3 moveToPosition;
    public GameObject target;
    public float distanceTarget;
    public bool isAttacking;
}
