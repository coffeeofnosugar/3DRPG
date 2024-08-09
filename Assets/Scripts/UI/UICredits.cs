using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UICredits : MonoBehaviour
    {
        [SerializeField] private MultiInputButton _closeButton;
        [SerializeField] private Player.InputReader _inputReader;
        [SerializeField] private UICreditsRoller _creditsRoller;
        
        public Action Closed;
        
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnClickClosedButton);                          // 注册按钮关闭事件
            _inputReader.UICancelEvent += OnClickClosedButton;                              // 注册navigate关闭事件
            _inputReader.UINavigateEvent += _creditsRoller.ChangeRollSpeed;
            
            _creditsRoller.StartRolling(4);
            
            _closeButton.UpdateSelected();
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClickClosedButton);
            _inputReader.UICancelEvent -= OnClickClosedButton;
            _inputReader.UINavigateEvent -= _creditsRoller.ChangeRollSpeed;
            
            _creditsRoller.StopRoll();
        }
        
        private void OnClickClosedButton()
        {
            Closed.Invoke();
        }

        private void SetCreditsText()
        {
            
        }
    }
}