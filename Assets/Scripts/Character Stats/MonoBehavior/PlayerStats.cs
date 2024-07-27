using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerStats : CharacterStats
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public PlayerInputController playerInputController;
        [HideInInspector] public Transform cameraTransform;
        public const float CrouchSpeed = 1.5f;
        public readonly float Gravity = -9.8f;

        public float CrouchThreshold { get; } = 0f;
        public float StandThreshold { get; } = 1f;
        public float MidairThreshold { get; } = 2f;

        public int PlayerStateHash { get; } = Animator.StringToHash("PlayerState");
        public int VerticalSpeedHash { get; } = Animator.StringToHash("VerticalSpeed");
        public int HorizontalSpeedHash { get; } = Animator.StringToHash("HorizontalSpeed");
        public int TurnSpeedHash { get; } = Animator.StringToHash("TurnSpeed");
        public int IsFighting { get; } = Animator.StringToHash("isFighting");

        protected override void Awake()
        {
            base.Awake();
            characterController = GetComponent<CharacterController>();
            playerInputController = GetComponent<PlayerInputController>();
            cameraTransform = Camera.main.transform;
        }
    }
}