using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Tooltip("¹¥»÷·¶Î§")]
    public float attackRange = 1;

    [Tooltip("¹¥»÷ÀäÈ´Ê±¼ä")]
    public float coolDown = 2;

    [Tooltip("×îĞ¡ÉËº¦")]
    public int minDamge = 3;

    [Tooltip("×î´óÉËº¦")]
    public int maxDamge = 5;

    [Tooltip("»÷ÍË¾àÀë")]
    public float kickForce = 0;
}
