using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tools.CoffeeTools;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField, BoxGroup("���ƫ����")] private float xOffset = 0.3f;
        [SerializeField, BoxGroup("���ƫ����")] private float yOffset = 0.6f;
        [SerializeField, BoxGroup("���ƫ����")] private float zOffset = 1.9f;
        /// <summary>
        /// ������ٶ�
        /// </summary>
        [SerializeField, Range(1, 50)] private float speed = 1;
        /// <summary>
        /// ����ƶ��ٶ�
        /// </summary>
        [SerializeField, Range(0, 1)] private float linearSpeed = 1;

        private CharacterController _characterController;
        private float _xMouse, _yMouse;

        private void Awake()
        {
            Debugs.Show("�����ʼ��...");
            if (player == null)
                player = FindObjectOfType<PlayerStats>().transform;
            _characterController = player.GetComponent<CharacterController>();
            
            Debugs.Show("�����ʼ��...Done");
        }

        private void LateUpdate()
        {
            _xMouse += Input.GetAxis("Mouse X") * linearSpeed;
            _yMouse -= Input.GetAxis("Mouse Y") * linearSpeed;
            // ���ƴ�ֱ����ĽǶ�
            _yMouse = Mathf.Clamp(_yMouse, -30, 80);
            // ����
            // offset.z -= Input.GetAxis("Mouse ScrollWheel") * 10;
            // offset.z = Mathf.Clamp(offset.z, -2, -15);
            
            Quaternion targetRotation = Quaternion.Euler(_yMouse, _xMouse, 0);
            Vector3 targetPosition = player.position + targetRotation * new Vector3(xOffset, yOffset, -zOffset) + _characterController.center;
            speed = _characterController.velocity.sqrMagnitude > 0.01f ? Mathf.Lerp(speed, 10, 5f * Time.deltaTime): Mathf.Lerp(speed, 25, 5f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 25 * Time.deltaTime);
        }
        
    //     private void CamCheck(out RaycastHit raycast, out float dis)
    //     {
    //         //���������ײ
    // #if UNITY_EDITOR
    //         Debug.DrawLine(player.position + player.GetComponent<CapsuleCollider>().center * 1.75f,
    //             player.position + player.GetComponent<CapsuleCollider>().center * 1.75f +
    //             (transform.position - player.position - player.GetComponent<CapsuleCollider>().center * 1.75f).normalized * distanceFromTarget
    //             , Color.blue);
    // #endif
    //         //�����ײ�����壬��ȡ��ײ����Ϣ�����¼�����룬���򷵻�Ĭ��ֵ
    //         if (Physics.Raycast(player.position + player.GetComponent<CapsuleCollider>().center * 1.75f,
    //                 (transform.position - player.position - player.GetComponent<CapsuleCollider>().center * 1.75f).normalized, out raycast,
    //                 distanceFromTarget, ~Physics.IgnoreRaycastLayer))
    //         {
    //             dis = Vector3.Distance(player.position + player.GetComponent<CapsuleCollider>().center * 1.75f + new Vector3(xOffset, 0, 0), raycast.point);
    //         }
    //         else
    //             dis = distanceFromTarget;
    //     }
    }
}