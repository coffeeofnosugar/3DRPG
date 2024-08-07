using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI
{
    public enum LanguageType { Chinese, English, Japanese }
    public enum DebugModeType { On, Off }

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
        /// 作用：防止语言还没有切换完成就再次切换语音
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
            debugModeItem.ChangeOption += ChangeDebugMode;
            
            languageItem.button.UpdateSelected();       // 必须在初始化所有完之后在更改选择，不然不会改变isSelected
        }
        private void OnDisable()
        {
            languageItem.button.SelectThisButton -= languageItem.ControllerThisItem;
            languageItem.ChangeOption -= ChangeLanguage;
            
            debugModeItem.button.SelectThisButton -= debugModeItem.ControllerThisItem;
            debugModeItem.ChangeOption -= ChangeDebugMode;
        }

        private void InitializeCompleted(AsyncOperationHandle obj)
        {
            _initializeOperation.Completed -= InitializeCompleted;
            languageItem.InitializedItem(1, 3, "Current(zh)");
            debugModeItem.InitializedItem(1, 2, "Yes");
        }

        #region SystemSettings

        
        private void ChangeLanguage(bool @bool)
        {
            SettingLanguage((LanguageType)(languageItem.paginationFiller.currentPagination - 1));
        }


        /// <summary>
        /// 设置语言
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


        private void ChangeDebugMode(bool @bool)
        {
            SettingDebugMode((DebugModeType)(debugModeItem.paginationFiller.currentPagination - 1));
        }

        private void SettingDebugMode(DebugModeType type)
        {
            Debug.Log($"Debug Mode Setting: {type}");
        }

        #endregion
    }
}
