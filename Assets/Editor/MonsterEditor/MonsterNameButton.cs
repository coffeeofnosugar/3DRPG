using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MonsterEditor
{
    public class MonsterNameButton : UnityEngine.UIElements.Button
    {
        public System.Action<CharacterData_SO> OnButtonClick;
        CharacterData_SO so;

        public MonsterNameButton(CharacterData_SO so)
        {
            this.so = so;
            this.name = "MonsterNameButton";
            this.text = so.monsterName;
            this.clicked += () => ShowCharacterData();
        }

        private void ShowCharacterData()
        {
            OnButtonClick?.Invoke(so);
        }
    }
}