using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player.PlayerController
{
    public class PlayerControllerRunner : MonoBehaviour
    {
        public PlayerController controller;

        private CharacterStats _characterStats;

        private void Awake()
        {
            // if (controller == null)
            // {
            //     Debug.LogError($"{transform.name} Œ¥≈‰÷√øÿ÷∆∆˜");
            //     return;
            // }
            // _characterStats = GetComponent<CharacterStats>();
        }

        private void Start()
        {
            var listener = ScriptableObject.CreateInstance<LogicKeyListener>();

            
            controller = ScriptableObject.CreateInstance<PlayerController>();
            controller.rootNode = listener;
        }

        private void FixedUpdate()
        {
            
        }

        private void Update()
        {
            controller.Update();
        }

        private void LateUpdate()
        {
            
        }
    }
}