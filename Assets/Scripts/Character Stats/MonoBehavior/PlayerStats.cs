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

        public float CrouchThreshold { get; } = 0f;
        public float StandThreshold { get; } = 1f;
        public float MidairThreshold { get; } = 2f;

        public int VerticalSpeedHash { get; } = Animator.StringToHash("VerticalSpeed");
        public int HorizontalSpeedHash { get; } = Animator.StringToHash("HorizontalSpeed");
        public int TurnSpeedHash { get; } = Animator.StringToHash("TurnSpeed");
        public int IsFighting { get; } = Animator.StringToHash("isFighting");

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
            PlayerInputController = GetComponent<PlayerInputController>();
            cameraTransform = Camera.main.transform;
        }
    }
}