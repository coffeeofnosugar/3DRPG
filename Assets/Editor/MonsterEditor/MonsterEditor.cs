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
        /// 添加怪物
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void AddButton_onClick()
        {
            var so = ScriptableObject.CreateInstance<CharacterData_SO>();
            var b = new MonsterNameButton(so);
            leftPanel.Add(b);
            // 创建
            AssetDatabase.CreateAsset(so, $"Assets/Game Data/Charater Data/{so.monsterName} Data.asset");
            // 保存
            AssetDatabase.SaveAssets();
            // 编译刷新
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 展示所有怪物
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
        /// 寻找指定路径下所有指定ScriptableObject类的文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static T[] FindAllScriptableObjects<T>(string folderPath) where T : ScriptableObject
        {
            // 查找指定文件夹中的所有资产路径
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { folderPath });

            // 将GUID转换为资产路径，然后加载资产
            T[] assets = guids.Select(guid =>
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }).ToArray();
            return assets;
        }
    }
}
