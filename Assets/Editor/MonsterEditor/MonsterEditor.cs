using Codice.Client.Common;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
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
            idIntegerField.bindingPath = "id";
            nameTextField = root.Q<TextField>("NameTextField");
            nameTextField.bindingPath = "monsterName";
            prefabTextField = root.Q<TextField>("PrefabTextField");
            prefabTextField.bindingPath = "prefab";

            maxHealthIntegerField = root.Q<IntegerField>("MaxHealthIntegerField");
            maxHealthIntegerField.bindingPath = "maxHealth";
            baseDefenceIntegerField = root.Q<IntegerField>("BaseDefenceIntegerField");
            baseDefenceIntegerField.bindingPath = "baseDefence";
            walkSpeedFloatField = root.Q<FloatField>("WalkSpeedFloatField");
            walkSpeedFloatField.bindingPath = "walkSpeed";
            runSpeedFloatField = root.Q<FloatField>("RunSpeedFloatField");
            runSpeedFloatField.bindingPath = "runSpeed";
            patrolRangeFloatField = root.Q<FloatField>("PatrolRangeFloatField");
            patrolRangeFloatField.bindingPath = "patrolRange";
            sightRangeFloatField = root.Q<FloatField>("SightRangeFloatField");
            sightRangeFloatField.bindingPath = "sightRadius";
            criticalMultiplierFloatField = root.Q<FloatField>("CriticalMultiplierFloatField");
            criticalMultiplierFloatField.bindingPath = "criticalMultiplier";
            criticalChanceFloatField = root.Q<FloatField>("CriticalChanceFloatField");
            criticalChanceFloatField.bindingPath = "criticalChance";
            destoryTimeIntegerField = root.Q<IntegerField>("DestoryTimeIntegerField");
            destoryTimeIntegerField.bindingPath = "destoryTime";

            // 添加菜单绑定事件
            addButton.clicked += AddButton_onClick;
            // 展示所有怪物的按钮
            ShowListButton();
            // 获取第一个怪物按钮并展示其内容
            var button = root.Q<MonsterNameButton>("MonsterNameButton");
            button.ShowCharacterData();

            addSkillButton.clicked += () => {
                // 创建AttackData_SO
                SkillData_SO skill = currentMonster.CreateSkill();
                CreateSkillView(skill);
            };
        }

        /// <summary>
        /// 添加怪物
        /// </summary>
        private void AddButton_onClick()
        {
            var character = ScriptableObject.CreateInstance<CharacterData_SO>();
            var b = new MonsterNameButton(character);
            leftPanel.Add(b);
            // 创建
            AssetDatabase.CreateAsset(character, $"Assets/Game Data/Charater Data/{character.monsterName} Data.asset");
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

        private void ShowCharacterData(CharacterData_SO character)
        {
            currentMonster = character;

            idIntegerField.value = character.id;
            nameTextField.value = character.monsterName;
            prefabTextField.value = character.prefab;
            maxHealthIntegerField.value = character.maxHealth;
            baseDefenceIntegerField.value = character.baseDefence;
            walkSpeedFloatField.value = character.walkSpeed;
            runSpeedFloatField.value = character.runSpeed;
            patrolRangeFloatField.value = character.patrolRange;
            sightRangeFloatField.value = character.sightRadius;
            criticalMultiplierFloatField.value = character.criticalMultiplier;
            criticalChanceFloatField.value = character.criticalChance;
            destoryTimeIntegerField.value = character.destoryTime;

            SerializedObject so = new SerializedObject(character);
            idIntegerField.Bind(so);
            nameTextField.Bind(so);
            prefabTextField.Bind(so);
            maxHealthIntegerField.Bind(so);
            baseDefenceIntegerField.Bind(so);
            walkSpeedFloatField.Bind(so);
            runSpeedFloatField.Bind(so);
            patrolRangeFloatField.Bind(so);
            sightRangeFloatField.Bind(so);
            criticalMultiplierFloatField.Bind(so);
            criticalChanceFloatField.Bind(so);
            destoryTimeIntegerField.Bind(so);

            skillView.Clear();
            for (int i = 0; i < character.skillList.Count; i++)
            {
                CreateSkillView(character.skillList[i]);
            }
        }

        private void CreateSkillView(SkillData_SO skill)
        {
            // 创建技能视图
            MonsterSkillView monsterSkillView = new MonsterSkillView(skill);
            monsterSkillView.DeleteSkillButton_onClick = () => {
                skillView.Remove(monsterSkillView);
                currentMonster.DeleteSkill(skill);
            };
            skillView.Add(monsterSkillView);
        }

        /// <summary>
        /// 寻找指定路径下所有指定ScriptableObject类的文件
        /// </summary>c
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
