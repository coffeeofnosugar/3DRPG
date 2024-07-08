using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Tooltip("¹¥»÷·¶Î§")]
    public float attackRange;

    [Tooltip("¹¥»÷ÀäÈ´Ê±¼ä")]
    public float coolDown;

    [Tooltip("×îĞ¡ÉËº¦")]
    public int minDamge;

    [Tooltip("×î´óÉËº¦")]
    public int maxDamge;

    [Tooltip("»÷ÍË¾àÀë")]
    public float kickForce;
}
