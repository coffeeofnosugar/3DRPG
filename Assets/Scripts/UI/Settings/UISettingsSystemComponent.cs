using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI
{
    public enum LanguageType { Chinese, English, Japanese }

    public class UISettingsSystemComponent : MonoBehaviour
    {
        public UISettingItemFiller languageItem;
        public UISettingItemFiller debugModeItem;
        
        public void LeftOrRightMove(Vector2 v)
        {
            if (languageItem.isSelected)
            {
                if (v.x >= .3f)
                    languageItem.NavigateUpdateOptions(false);
                else if (v.x <= -.3f)
                    languageItem.NavigateUpdateOptions(true);
            }
            else if (debugModeItem.isSelected)
            {
                if (v.x >= .3f)
                    debugModeItem.NavigateUpdateOptions(false);
                else if (v.x <= -.3f)
                    debugModeItem.NavigateUpdateOptions(true);
            }
        }
        
        /// <summary>
        /// ���ã���ֹ���Ի�û���л���ɾ��ٴ��л�����
        /// </summary>
        private AsyncOperationHandle _initializeOperation;
        private void OnEnable()
        {
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;
            if (_initializeOperation.IsDone)
                InitializeCompleted(_initializeOperation);
            else
                _initializeOperation.Completed += InitializeCompleted;
            
            languageItem.button.SelectThisButton += languageItem.ControllerThisItem;
            languageItem.ChangeOption += ChangeLanguage;
            
            debugModeItem.button.SelectThisButton += debugModeItem.ControllerThisItem;
            debugModeItem.ChangeOption += ChangeLanguage;
            
            languageItem.button.UpdateSelected();       // �����ڳ�ʼ��������֮���ڸ���ѡ�񣬲�Ȼ����ı�isSelected
        }
        private void OnDisable()
        {
            languageItem.button.SelectThisButton -= languageItem.ControllerThisItem;
            languageItem.ChangeOption -= ChangeLanguage;
        }

        private void InitializeCompleted(AsyncOperationHandle obj)
        {
            _initializeOperation.Completed -= InitializeCompleted;
            languageItem.InitializedItem(1, 3, "Current(zh)");
            debugModeItem.InitializedItem(1, 2, "Yes");
        }

        private void ChangeLanguage(bool @bool)
        {
            Debug.Log($"Move: {@bool}");
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="type"></param>
        private void SettingLanguage(LanguageType type)
        {
            var locals = LocalizationSettings.AvailableLocales.Locales;
            foreach (var local in locals.Where(local => local.LocaleName == type.ToString()))
            {
                LocalizationSettings.SelectedLocale = local;
            }
        }


        // private bool Vector2ToBool(Vector2 vector2)
        // {
        //     if (vector2.x >= .3f)
        //         return false;
        //     else if (vector2.x <= .3f)
        //         return false;
        //     else
        //         
        // }
    }
}
