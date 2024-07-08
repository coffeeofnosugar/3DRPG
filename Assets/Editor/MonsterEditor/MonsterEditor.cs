using Codice.Client.Common;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterEditor
{
    public class MonsterEditor : EditorWindow
    {
        Button addButton, deleteButton, updateButton;

        IntegerField idIntegerField;
        TextField nameTextField, prefabTextField;


        IntegerField maxHealthIntegerField, baseDefenceIntegerField, walkSpeedIntegerField, runSpeedIntegerField, patrolRangeIntegerField, sightRangeIntegerField, criticalMultiplierIntegerField, criticalChanceIntegerField, destoryTimeIntegerField;
        
        VisualElement leftPanel;


        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Tools/MonsterEditor")]
        public static void OpenWindow()
        {
            MonsterEditor wnd = GetWindow<MonsterEditor>();
            wnd.titleContent = new GUIContent("MonsterEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            m_VisualTreeAsset.CloneTree(root);

            addButton = root.Q<Button>("AddToolbarButton");
            deleteButton = root.Q<Button>("DeleteToolbarButton");
            updateButton = root.Q<Button>("UpdateToolbarButton");
            leftPanel = root.Q<VisualElement>("left-panel");

            idIntegerField = root.Q<IntegerField>("IdIntegerField");
            nameTextField = root.Q<TextField>("NameTextField");
            prefabTextField = root.Q<TextField>("PrefabTextField");

            maxHealthIntegerField = root.Q<IntegerField>("MaxHealthIntegerField");
            baseDefenceIntegerField = root.Q<IntegerField>("BaseDefenceIntegerField");
            walkSpeedIntegerField = root.Q<IntegerField>("WalkSpeedIntegerField");
            runSpeedIntegerField = root.Q<IntegerField>("RunSpeedIntegerField");
            patrolRangeIntegerField = root.Q<IntegerField>("PatrolRangeIntegerField");
            sightRangeIntegerField = root.Q<IntegerField>("SightRangeIntegerField");
            criticalMultiplierIntegerField = root.Q<IntegerField>("CriticalMultiplierIntegerField");
            criticalChanceIntegerField = root.Q<IntegerField>("CriticalChanceIntegerField");
            destoryTimeIntegerField = root.Q<IntegerField>("DestoryTimeIntegerField");

            addButton.clicked += AddButton_onClick;
            ShowListButton();
        }

        /// <summary>
        /// ��ӹ���
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void AddButton_onClick()
        {
            var so = ScriptableObject.CreateInstance<CharacterData_SO>();
            var b = new MonsterNameButton(so);
            leftPanel.Add(b);
            // ����
            AssetDatabase.CreateAsset(so, $"Assets/Game Data/Charater Data/{so.monsterName} Data.asset");
            // ����
            AssetDatabase.SaveAssets();
            // ����ˢ��
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// չʾ���й���
        /// </summary>
        private void ShowListButton()
        {
            var sos = FindAllScriptableObjects<CharacterData_SO>("Assets/Game Data/Charater Data/");

            for (int i = 0; i < sos.Length; i++)
            {
                var b = new MonsterNameButton(sos[i]);
                leftPanel.Add(b);
                b.OnButtonClick = ShowCharacterData;
            }
        }

        private void ShowCharacterData(CharacterData_SO so)
        {
            nameTextField.value = so.monsterName;
        }
        /// <summary>
        /// Ѱ��ָ��·��������ָ��ScriptableObject����ļ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static T[] FindAllScriptableObjects<T>(string folderPath) where T : ScriptableObject
        {
            // ����ָ���ļ����е������ʲ�·��
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { folderPath });

            // ��GUIDת��Ϊ�ʲ�·����Ȼ������ʲ�
            T[] assets = guids.Select(guid =>
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }).ToArray();
            return assets;
        }
    }
}
