using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public bool isOpenAI;
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
    public float destoryTime;
}
