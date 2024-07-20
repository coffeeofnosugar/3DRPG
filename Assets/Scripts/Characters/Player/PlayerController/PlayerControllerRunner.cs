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
            //     Debug.LogError($"{transform.name} δ���ÿ�����");
            //     return;
            // }
            _characterStats = GetComponent<CharacterStats>();
            _playerInputController = GetComponent<PlayerInputController>();
        }

        private void Start()
        {
            controller = controller.Clone();
            controller.Bind(_characterStats, _playerInputController);
        }

        private void Update()
        {
            controller.Update();
        }
    }
}