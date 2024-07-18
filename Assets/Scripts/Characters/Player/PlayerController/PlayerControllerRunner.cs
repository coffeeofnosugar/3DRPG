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
        private PlayerInputController _playerInputController;

        private void Awake()
        {
            // if (controller == null)
            // {
            //     Debug.LogError($"{transform.name} Œ¥≈‰÷√øÿ÷∆∆˜");
            //     return;
            // }
            _characterStats = GetComponent<CharacterStats>();
            _playerInputController = GetComponent<PlayerInputController>();
        }

        private void Start()
        {
            controller = ScriptableObject.CreateInstance<PlayerController>();
            
            var root = ScriptableObject.CreateInstance<Root>();
            var listener = ScriptableObject.CreateInstance<LogicKeyListener>();
            listener._playerInputController = _playerInputController;
            // var log = ScriptableObject.CreateInstance<Log>();
            
            controller.rootNode = root;
            root.child = listener;

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