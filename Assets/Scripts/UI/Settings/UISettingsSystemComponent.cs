using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI
{
    public enum LanguageType { Chinese, English, Japanese }

    public class UISettingsSystemComponent : MonoBehaviour
    {
        public UISettingItemFiller languageItem;
        // public UISettingItemFiller debugModeItem;
        
        /// <summary>
        /// 左为true，右为false
        /// </summary>
        public Action<Vector2> LeftOrRightMoveComponent;

        public void LeftOrRightMove(Vector2 v)
        {
            // if (v.x >= .3f)
            //     languageItem.UpdateOptions(false);
            // else if (v.x <= -.3f)
            //     languageItem.UpdateOptions(true);
        }
        
        /// <summary>
        /// 作用：防止语言还没有切换完成就再次切换语音
        /// </summary>
        private AsyncOperationHandle _initializeOperation;
        private void OnEnable()
        {
            languageItem.button.UpdateSelected();
            
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;
            if (_initializeOperation.IsDone)
                InitializeCompleted(_initializeOperation);
            else
                _initializeOperation.Completed += InitializeCompleted;
        }

        private void InitializeCompleted(AsyncOperationHandle obj)
        {
            _initializeOperation.Completed -= InitializeCompleted;
            languageItem.InitializedItem(1, 3, "Current(zh)");
            // debugModeItem.InitializedItem(1, 2, "Yes");
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
