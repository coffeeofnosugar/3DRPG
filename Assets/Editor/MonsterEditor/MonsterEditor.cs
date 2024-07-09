using Codice.Client.Common;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

namespace MonsterEditor
{
    public class MonsterEditor : EditorWindow
    {
        VisualElement leftPanel, skillView;
        Button addButton, deleteButton, updateButton, addSkillButton;

        IntegerField idIntegerField;
        TextField nameTextField, prefabTextField;

        CharacterData_SO currentMonster;

        IntegerField maxHealthIntegerField, baseDefenceIntegerField;
        FloatField walkSpeedFloatField, runSpeedFloatField, patrolRangeFloatField, sightRangeFloatField, criticalMultiplierFloatField, criticalChanceFloatField;
        IntegerField destoryTimeIntegerField;

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

            leftPanel = root.Q<VisualElement>("left-panel");
            skillView = root.Q<VisualElement>("skill-view");

            addButton = root.Q<Button>("AddToolbarButton");
            deleteButton = root.Q<Button>("DeleteToolbarButton");
            updateButton = root.Q<Button>("UpdateToolbarButton");
            addSkillButton = root.Q<Button>("AddSkillButton");

            idIntegerField = root.Q<IntegerField>("IdIntegerField");
            nameTextField = root.Q<TextField>("NameTextField");
            prefabTextField = root.Q<TextField>("PrefabTextField");

            maxHealthIntegerField = root.Q<IntegerField>("MaxHealthIntegerField");
            baseDefenceIntegerField = root.Q<IntegerField>("BaseDefenceIntegerField");
            walkSpeedFloatField = root.Q<FloatField>("WalkSpeedFloatField");
            runSpeedFloatField = root.Q<FloatField>("RunSpeedFloatField");
            patrolRangeFloatField = root.Q<FloatField>("PatrolRangeFloatField");
            sightRangeFloatField = root.Q<FloatField>("SightRangeFloatField");
            criticalMultiplierFloatField = root.Q<FloatField>("CriticalMultiplierFloatField");
            criticalChanceFloatField = root.Q<FloatField>("CriticalChanceFloatField");
            destoryTimeIntegerField = root.Q<IntegerField>("DestoryTimeIntegerField");

            // 添加菜单绑定事件
            addButton.clicked += AddButton_onClick;
            // 展示所有怪物的按钮
            ShowListButton();
            // 获取第一个怪物按钮并展示其内容
            var button = root.Q<MonsterNameButton>("MonsterNameButton");
            button.ShowCharacterData();

            addSkillButton.clicked += () => {
                // 创建AttackData_SO
                AttackData_SO so = currentMonster.CreateAttackData_SO();
                CreateSkillView(so);
            };
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
                // 注册点击事件，执行ShowCharacterData方法
                b.OnButtonClick = ShowCharacterData;
                leftPanel.Add(b);
            }
        }

        private void ShowCharacterData(CharacterData_SO so)
        {
            currentMonster = so;
            idIntegerField.value = so.id;
            nameTextField.value = so.monsterName;
            prefabTextField.value = so.prefab;
            maxHealthIntegerField.value = so.maxHealth;
            baseDefenceIntegerField.value = so.baseDefence;
            walkSpeedFloatField.value = so.walkSpeed;
            runSpeedFloatField.value = so.runSpeed;
            patrolRangeFloatField.value = so.patrolRange;
            sightRangeFloatField.value = so.sightRadius;
            criticalMultiplierFloatField.value = so.criticalMultiplier;
            criticalChanceFloatField.value = so.criticalChance;
            destoryTimeIntegerField.value = so.destoryTime;

            skillView.Clear();
            for (int i = 0; i < so.skillList.Count; i++)
            {
                CreateSkillView(so.skillList[i]);
            }
        }

        private void CreateSkillView(AttackData_SO so)
        {
            // 创建视图
            MonsterSkillView monsterSkillView = new MonsterSkillView(so);
            skillView.Add(monsterSkillView);
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
