using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace UI
{
    public class UISettingItemFiller : MonoBehaviour
    {
        [SerializeField, BoxGroup] private string localizeKey;
        [SerializeField, BoxGroup] public MultiInputButton button;
        [SerializeField, BoxGroup] private LocalizeStringEvent _title;
        [SerializeField, BoxGroup] private LocalizeStringEvent currentOptionsText;
        [SerializeField, BoxGroup] private UIPaginationFiller paginationFiller;
        [SerializeField, BoxGroup] private Button buttonPrevious;
        [SerializeField, BoxGroup] private Button buttonNext;

        [Button]
        public void Func()
        {
            Debug.Log(_title.StringReference.TableEntryReference);
        }

        public void InitializedItem(int selectedIndex, int paginationCount, string selectedOptions)
        {
            Debug.Log("setting title localizeKey: " + localizeKey);
            try
            {
                _title.StringReference.TableEntryReference = localizeKey;
                Debug.Log(_title.StringReference.TableEntryReference);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            currentOptionsText.StringReference.TableEntryReference = localizeKey + "_" + selectedOptions;
            paginationFiller.InitializedPagination(selectedIndex, paginationCount);

            InteractableButton();
            
            buttonPrevious.onClick.AddListener(() => { UpdateOptions(true);});
            buttonNext.onClick.AddListener(() => { UpdateOptions(false);});
            Debug.Log("初始化完成");
        }
        
        /// <summary>
        /// 左为true，右为true
        /// </summary>
        public Action<bool> LeftOrRightMove;

        /// <summary>
        /// 左为true，右为false
        /// </summary>
        /// <param name="bool"></param>
        public void UpdateOptions(bool @bool)
        {
            // LeftOrRightMove(@bool);
            if (@bool)
                paginationFiller.MoveLeftPagination();
            else
                paginationFiller.MoveRightPagination();
            InteractableButton();
        }

        /// <summary>
        /// 检测左右按钮是否可用
        /// </summary>
        private void InteractableButton()
        {
            buttonPrevious.interactable = paginationFiller.currentPagination > 1;
            buttonNext.interactable = paginationFiller.currentPagination < paginationFiller.totalPagination;
        }
    }
}