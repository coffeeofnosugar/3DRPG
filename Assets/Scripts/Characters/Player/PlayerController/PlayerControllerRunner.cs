using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player.PlayerController
{
    public class PlayerControllerRunner : MonoBehaviour
    {
        public PlayerController controller;

        private PlayerStats playerStats;
        private PlayerInputController _playerInputController;

        private void Awake()
        {
            // if (controller == null)
            // {
            //     Debug.LogError($"{transform.name} Œ¥≈‰÷√øÿ÷∆∆˜");
            //     return;
            // }
            playerStats = GetComponent<PlayerStats>();
            _playerInputController = GetComponent<PlayerInputController>();
        }

        private void Start()
        {
            controller = controller.Clone();
            controller.Bind(playerStats, _playerInputController);
        }
        
        private void FixedUpdate()
        {
            controller.FixedUpdate();
        }
    }
}