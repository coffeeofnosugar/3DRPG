using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UIPaginationFiller : MonoBehaviour
    {
        [SerializeField] private Image _imagePaginationPrefab;
        [SerializeField] private Sprite _emptyPagination;
        [SerializeField] private Sprite _filledPagination;

        [SerializeField, ReadOnly] public int currentPagination;
        [SerializeField, ReadOnly] public int totalPagination;
        [SerializeField, ReadOnly] private List<Image> _instantiatedImages = new List<Image>();

        public void InitializedPagination(int selectedIndex, int paginationCount)
        {
            if (_instantiatedImages.Count <= 0)
            {
                totalPagination = paginationCount;
                for (int i = 0; i < paginationCount; i++)
                {
                    var image = Instantiate(_imagePaginationPrefab, transform);
                    _instantiatedImages.Add(image);
                }
            }
            UpdatePagination(selectedIndex);
        }

        /// <summary>
        /// 设置当前个数
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private void UpdatePagination(int index)
        {
            if (index > 0 && index <= _instantiatedImages.Count)
            {
                currentPagination = index;
                for (int i = 0; i < _instantiatedImages.Count; i++)
                {
                    _instantiatedImages[i].sprite = index - 1 == i ? _filledPagination : _emptyPagination;
                }
            }
        }

        public void MovePagination(bool @bool)
        {
            if (@bool)
                UpdatePagination(currentPagination - 1);
            else
                UpdatePagination(currentPagination + 1);
            Debug.Log($"currentPagination: {currentPagination}");
        }
    }
}