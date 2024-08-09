using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.CoffeeTools
{
    public class Debugs : MonoSingleton<Debugs>
    {
        [SerializeField] private Transform _scrollViewContent;
        [SerializeField] private GameObject _textPrefab;
        [SerializeField] private TMP_Text updateText;
        public static UpdateLogText UpdateLogText;
        public enum DebugTypeEnum { Normal, WithoutUnityDebug }

        private new void Awake()
        {
            base.Awake();
            // UpdateLogText = new UpdateLogText(updateText);
        }

        public static void Show(object log, DebugTypeEnum type = DebugTypeEnum.WithoutUnityDebug)
        {
            GameObject obj = Instantiate(Instance._textPrefab, Instance._scrollViewContent);
            obj.GetComponent<Text>().text = DateTime.Now + "  " + log.ToString();
            if (type != DebugTypeEnum.WithoutUnityDebug)
                Debug.Log(log);
        }
    }

    public class UpdateLogText
    {
        private static TMP_Text _updateText;

        private Dictionary<string, string> _data = new Dictionary<string, string>();

        private StringBuilder _stringBuilder;
        
        public object this[string key]
        {
            get => _data[key];
            set
            {
                if (_data.ContainsKey(key))
                    _data[key] = value.ToString();
                else
                    _data.Add(key, value.ToString());
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
