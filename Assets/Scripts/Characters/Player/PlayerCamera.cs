using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float xOffset = 0.3f;
        [SerializeField] private float yOffset = 0.6f;
        [SerializeField] private float zOffset = 1.9f;
        /// <summary>
        /// 摄像机速度
        /// </summary>
        [SerializeField, Range(1, 50)] private float speed = 1;
        /// <summary>
        /// 鼠标移动速度
        /// </summary>
        [SerializeField, Range(0, 1)] private float linearSpeed = 1;
        
        private CapsuleCollider _capsuleCollider;
        private Rigidbody _rigidbody;
        public float _xMouse, _yMouse;

        private void Awake()
        {
            _rigidbody = player.GetComponent<Rigidbody>();
            _capsuleCollider = player.GetComponent<CapsuleCollider>();
        }

        private void LateUpdate()
        {
            _xMouse += Input.GetAxis("Mouse X") * linearSpeed;
            _yMouse -= Input.GetAxis("Mouse Y") * linearSpeed;
            // 限制垂直方向的角度
            _yMouse = Mathf.Clamp(_yMouse, -30, 80);
            // 滑轮
            // offset.z -= Input.GetAxis("Mouse ScrollWheel") * 10;
            // offset.z = Mathf.Clamp(offset.z, -2, -15);
            
            Quaternion targetRotation = Quaternion.Euler(_yMouse, _xMouse, 0);
            Vector3 targetPostion = player.position + targetRotation * new Vector3(xOffset, yOffset, -zOffset) + _capsuleCollider.center;
            speed = _rigidbody.velocity.sqrMagnitude > 0.01f ? Mathf.Lerp(speed, 10, 5f * Time.deltaTime): Mathf.Lerp(speed, 25, 5f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPostion, speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 25 * Time.deltaTime);
        }
        
    //     private void CamCheck(out RaycastHit raycast, out float dis)
    //     {
    //         //用来检测碰撞
    // #if UNITY_EDITOR
    //         Debug.DrawLine(player.position + player.GetComponent<CapsuleCollider>().center * 1.75f,
    //             player.position + player.GetComponent<CapsuleCollider>().center * 1.75f +
    //             (transform.position - player.position - player.GetComponent<CapsuleCollider>().center * 1.75f).normalized * distanceFromTarget
    //             , Color.blue);
    // #endif
    //         //如果碰撞到物体，获取碰撞点信息，重新计算距离，否则返回默认值
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