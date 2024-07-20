using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڰ壬�洢һЩ��Ϊ�����õ��ı���
/// </summary>
[System.Serializable]
public class Blackboard
{
    public Vector3 moveToPosition;
    public GameObject target;
    public float distanceTarget;
    public bool isAttacking;
    public bool getHit;
    public bool isDeath;
    public Dictionary<string, float> lastAttackTime = new Dictionary<string, float>();
}
