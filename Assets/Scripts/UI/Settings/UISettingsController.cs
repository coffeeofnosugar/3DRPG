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

        [SerializeField] private Player.InputReader _inputReader;

        
        public Action Closed;


        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnClickClosedButton);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClickClosedButton);
        }

        private void OnClickClosedButton() { Closed.Invoke(); }
    }
}