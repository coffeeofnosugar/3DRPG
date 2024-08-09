using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UISettingsController : MonoBehaviour
    {
        [SerializeField, BoxGroup("LanguageTab")] private MultiInputButton _closeButton;
        [SerializeField, BoxGroup("LanguageTab")] private UISettingsSystemComponent settingsSystemComponent;

        [SerializeField, InlineEditor] private Player.InputReader _inputReader;

        
        public Action Closed;


        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnClickClosedButton);                          // 注册按钮关闭事件
            _inputReader.UICancelEvent += OnClickClosedButton;                              // 注册navigate关闭事件
            _inputReader.UINavigateEvent += settingsSystemComponent.LeftOrRightMove;        // 注册左右更改设置按钮
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClickClosedButton);
            _inputReader.UICancelEvent -= OnClickClosedButton;
            _inputReader.UINavigateEvent -= settingsSystemComponent.LeftOrRightMove;
        }

        private void OnClickClosedButton() { Closed.Invoke(); }
    }
}