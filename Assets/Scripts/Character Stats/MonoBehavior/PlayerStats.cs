using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStats : CharacterStats
    {
        [HideInInspector] public new Rigidbody rigidbody;
        [HideInInspector] public PlayerInputController PlayerInputController;
        [HideInInspector] public Transform cameraTransform;

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
            PlayerInputController = GetComponent<PlayerInputController>();
            cameraTransform = Camera.main.transform;
        }
    }
}