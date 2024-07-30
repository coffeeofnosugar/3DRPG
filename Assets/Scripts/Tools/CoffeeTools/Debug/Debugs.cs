using System;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.CoffeeTools
{
    public class Debugs : UnitySingleton<Debugs>
    {
        [SerializeField] private Transform _scrollViewContent;
        [SerializeField] private GameObject _textPrefab;
        [SerializeField] private TMP_Text updateText;
        public static UpdateLogText UpdateLogText;

        private void Awake()
        {
            UpdateLogText = new UpdateLogText(updateText);
        }

        public static void Show(string log)
        {
            GameObject obj = Instantiate(Debugs.Instance._textPrefab, Debugs.Instance._scrollViewContent);
            obj.GetComponent<Text>().text = log;
        }
    }

    public class UpdateLogText
    {
        private static TMP_Text _updateText;

        private Dictionary<string, string> _data = new Dictionary<string, string>();

        private StringBuilder _stringBuilder;
        
        public string this[string key]
        {
            get => _data[key];
            set
            {
                if (_data.ContainsKey(key))
                    _data[key] = value;
                else
                    _data.Add(key, value);
                UpdateContent();
            }
        }

        public UpdateLogText(TMP_Text text)
        {
            _updateText = text;
        }

        private void UpdateContent()
        {
            _stringBuilder = new StringBuilder();
            foreach (var item in _data)
            {
                _stringBuilder.Append(item.Key);
                _stringBuilder.Append(": ");
                _stringBuilder.AppendLine(item.Value);
            }

            _updateText.text = _stringBuilder.ToString();
        }
    }
}
