using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MonsterEditor
{
    public class MonsterNameButton : UnityEngine.UIElements.Button
    {
        public System.Action<CharacterData_SO> OnButtonClick;
        CharacterData_SO character;

        public MonsterNameButton(CharacterData_SO character)
        {
            this.character = character;
            this.name = "MonsterNameButton";
            this.text = character.monsterName;
            this.clicked += () => ShowCharacterData();
        }

        public void ShowCharacterData()
        {
            OnButtonClick?.Invoke(character);
        }
    }
}