using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerStats : CharacterStats
    {
        [HideInInspector] public new Rigidbody rigidbody;
        [HideInInspector] public PlayerInputController PlayerInputController;
        [HideInInspector] public Transform cameraTransform;

        public int playerStateHash;
        public int moveSpeedHash;

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
            PlayerInputController = GetComponent<PlayerInputController>();
            cameraTransform = Camera.main.transform;
            
            playerStateHash = Animator.StringToHash("PlayerState");
            moveSpeedHash = Animator.StringToHash("MoveSpeed");
        }
    }
}