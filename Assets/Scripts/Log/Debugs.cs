using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class Debugs : UnitySingleton<Debugs>
{
    public TMP_Text DebugText;

    private Dictionary<string, string> data = new Dictionary<string, string>() { };

    private StringBuilder str;

    public string this[string key]
    {
        set
        {
            if (data.ContainsKey(key))
                data[key] = value;
            else
                data.Add(key, value);
            UpdataContent();
        }
        get
        {
            return data[key];
        }
    }

    public void UpdataContent()
    {
        str = new StringBuilder();
        foreach (var item in data)
        {
            str.Append(item.Key);
            str.Append(": ");
            str.AppendLine(item.Value);
        }
        DebugText.text = str.ToString();
    }
}

public class MyDictionary
{
    public Dictionary<string, string> dic = new Dictionary<string, string>();

    public string this[string key]
    {
        set
        {
            dic[key] = value;
        }
        get
        {
            return dic[key];
        }
    }
}