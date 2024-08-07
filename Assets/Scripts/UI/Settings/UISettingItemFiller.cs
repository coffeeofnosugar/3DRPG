using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField, BoxGroup] public UIPaginationFiller paginationFiller;
        [SerializeField, BoxGroup] private Button buttonPrevious;
        [SerializeField, BoxGroup] private Button buttonNext;
        [ReadOnly, BoxGroup] public bool isSelected;        // EventSystem.Currentѡ����Ƿ����Լ�

        public Action<bool> ChangeOption;

        public void InitializedItem(int selectedIndex, int paginationCount, string selectedOptions)
        {
            _title.StringReference.TableEntryReference = localizeKey;
            currentOptionsText.StringReference.TableEntryReference = localizeKey + "_" + selectedOptions;
            paginationFiller.InitializedPagination(selectedIndex, paginationCount);

            InteractableButton();
            
            buttonPrevious.onClick.AddListener(() => { ButtonUpdateOptions(true).Forget();});
            buttonNext.onClick.AddListener(() => { ButtonUpdateOptions(false).Forget();});
            
            button.SelectThisButton += ControllerThisItem;
        }

        public void ControllerThisItem(bool @bool)
        {
            isSelected = @bool;
        }

        public void NavigateUpdateOptions(bool @bool)
        {
            if (isSelected)     // ѡ�����Ƿ����Լ����item
            {
                if (@bool && paginationFiller.currentPagination > 1)      // ֻ�е�ǰѡ��Ĳ��ǵ�һ��ʱ��������
                {
                    SimulationClickButton(@bool).Forget();
                    paginationFiller.MovePagination(@bool);
                    ChangeOption(@bool);
                }
                else if (!@bool && paginationFiller.currentPagination < paginationFiller.totalPagination)   // ֻ�е�ǰѡ��Ĳ������һ����������
                {
                    SimulationClickButton(@bool).Forget();
                    paginationFiller.MovePagination(@bool);
                    ChangeOption(@bool);
                }
            }
        }

        /// <summary>
        /// ��Ϊtrue����Ϊfalse
        /// </summary>
        /// <param name="bool"></param>
        private async UniTask ButtonUpdateOptions(bool @bool)
        {
            if (isSelected)     // ѡ�����Ƿ����Լ����item
            {
                if (@bool && paginationFiller.currentPagination > 1)      // ֻ�е�ǰѡ��Ĳ��ǵ�һ��ʱ��������
                {
                    paginationFiller.MovePagination(@bool);
                    ChangeOption(@bool);
                }
                else if (!@bool && paginationFiller.currentPagination < paginationFiller.totalPagination)   // ֻ�е�ǰѡ��Ĳ������һ����������
                {
                    paginationFiller.MovePagination(@bool);
                    ChangeOption(@bool);
                }
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(.1f), ignoreTimeScale: false);
            InteractableButton();                           // �ƶ�����ݵ�ǰλ�����ð�ť�Ƿ����
        }

        /// <summary>
        /// ģ�ⰴť������
        /// </summary>
        /// <param name="bool"></param>
        private async UniTask SimulationClickButton(bool @bool)
        {
            if (@bool)
            {
                buttonPrevious.Select();
                buttonPrevious.OnPointerDown(new PointerEventData(EventSystem.current));
                await UniTask.Delay(TimeSpan.FromSeconds(.1f), ignoreTimeScale: false);
                buttonPrevious.OnPointerUp(new PointerEventData(EventSystem.current));
            }
            else
            {
                buttonNext.Select();
                buttonNext.OnPointerDown(new PointerEventData(EventSystem.current));
                await UniTask.Delay(TimeSpan.FromSeconds(.1f), ignoreTimeScale: false);
                buttonNext.OnPointerUp(new PointerEventData(EventSystem.current));
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(.1f), ignoreTimeScale: false);
            button.Select();
            InteractableButton();
        }
        
        /// <summary>
        /// ������Ұ�ť�Ƿ����
        /// </summary>
        private void InteractableButton()
        {
            buttonPrevious.interactable = paginationFiller.currentPagination > 1;
            buttonNext.interactable = paginationFiller.currentPagination < paginationFiller.totalPagination;
        }
    }
}