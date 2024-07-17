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
            if (controller == null)
            {
                Debug.LogError($"{transform.name} Œ¥≈‰÷√øÿ÷∆∆˜");
                return;
            }
            _characterStats = GetComponent<CharacterStats>();
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
            
        }
    }
}