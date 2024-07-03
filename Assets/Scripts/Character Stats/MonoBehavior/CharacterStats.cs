using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    public bool isCritical;

    public bool getHit;

    #region Read from CharacterData_SO
    public bool IsOpenAI
    {
        get { if (characterData != null) { return characterData.isOpenAI; } else { return true; } }
        set { characterData.isOpenAI = value; }
    }
    public int MaxHealth
    {
        get { if (characterData != null) { return characterData.maxHealth; } else { return 0; } }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData != null) { return characterData.currentHealth; } else { return 0; } }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) { return characterData.baseDefence; } else { return 0; } }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData != null) { return characterData.currentDefence; } else { return 0; } }
        set { characterData.baseDefence = value; }
    }
    public float DestoryTime
    {
        get { if (characterData != null) { return characterData.destoryTime; } else { return 0; } }
        set { characterData.destoryTime = value; }
    }
    public bool IsDeath
    {
        get { if (characterData != null) { return characterData.isDeath; } else { return true; } }
        set { characterData.isDeath = value; }
    }
    #endregion

    #region Read fron AttackData_SO
    public float AttackRange
    {
        get { if (attackData != null) { return attackData.attackRange; } else { return 0; } }
        set { attackData.attackRange = value; }
    }
    public float SkillRange
    {
        get { if (attackData != null) { return attackData.skillRange; } else { return 0; } }
        set { attackData.skillRange = value; }
    }
    public float CoolDown
    {
        get { if (attackData != null) { return attackData.coolDown; } else { return 0; } }
        set { attackData.coolDown = value; }
    }
    public int MinDange
    {
        get { if (attackData != null) { return attackData.minDamge; } else { return 0; } }
        set { attackData.minDamge = value; }
    }
    public int MaxDange
    {
        get { if (attackData != null) { return attackData.maxDamge; } else { return 0; } }
        set { attackData.maxDamge = value; }
    }
    public float CriticalMultiplier
    {
        get { if (attackData != null) { return attackData.criticalMultiplier; } else { return 0; } }
        set { attackData.criticalMultiplier = value; }
    }
    public float CriticalChance
    {
        get { if (attackData != null) { return attackData.criticalChance; } else { return 0; } }
        set { attackData.criticalChance = value; }
    }
    #endregion




    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        defener.getHit = true;
        float coreDamage = Random.Range(attacker.MinDange, attacker.MaxDange);
        if (attacker.isCritical)
        {
            coreDamage *= attacker.CriticalMultiplier;
            Debug.Log("±©»÷£¡" + coreDamage);
        }

        int damage = Mathf.Max((int)coreDamage - defener.CurrentDefence, 0);
        defener.CurrentHealth = Mathf.Max(defener.CurrentHealth - damage, 0);
        // Update UI
        // ¾­ÑéUpdate
    }
}
