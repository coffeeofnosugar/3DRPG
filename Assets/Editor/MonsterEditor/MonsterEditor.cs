using Codice.Client.BaseCommands.CheckIn.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace MonsterEditor
{
    public class MonsterEditor : EditorWindow
    {
        VisualElement leftPanel, skillView;
        Button addButton, deleteButton, addSkillButton;

        IntegerField idIntegerField;
        TextField nameTextField;

        MonsterNameButton currentMonster;
        List<MonsterNameButton> monsterList = new List<MonsterNameButton>();

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

            // 添加菜单绑定事件
            addButton = root.Q<Button>("AddToolbarButton");
            addButton.clicked += AddButton_onClick;

            deleteButton = root.Q<Button>("DeleteToolbarButton");
            deleteButton.clicked += DeleteButton_onClick;

            addSkillButton = root.Q<Button>("AddSkillButton");
            addSkillButton.clicked += () => {
                // 创建SkillData_SO
                SkillData_SO skill = currentMonster.character.CreateSkill();
                CreateSkillView(skill);
            };

            idIntegerField = root.Q<IntegerField>("IdIntegerField");
            idIntegerField.bindingPath = "id";
            nameTextField = root.Q<TextField>("NameTextField");
            nameTextField.bindingPath = "monsterName";

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

            // 展示所有怪物的按钮
            ShowListButton();
            // 获取第一个怪物按钮并展示其内容
            monsterList[0].ShowCharacterData();
        }

        /// <summary>
        /// 添加怪物
        /// </summary>
        private void AddButton_onClick()
        {
            var character = ScriptableObject.CreateInstance<CharacterData_SO>();
            AssetDatabase.CreateAsset(character, $"Assets/Game Data/Monster Data/{character.monsterName} Data.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            var b = new MonsterNameButton(character);
            // 注册点击事件，执行ShowCharacterData方法
            b.OnButtonClick = ShowCharacterData;
            // 设置按钮位置
            leftPanel.Add(b);
            // 存储对象
            monsterList.Add(b);

            b.ShowCharacterData();
        }

        private void DeleteButton_onClick()
        {
            leftPanel.Remove(currentMonster);
            monsterList.Remove(currentMonster);
            AssetDatabase.DeleteAsset($"Assets/Game Data/Monster Data/{currentMonster.character.monsterName} Data.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 获取第一个怪物按钮并展示其内容
            monsterList[0].ShowCharacterData();
        }

        /// <summary>
        /// 展示所有怪物
        /// </summary>
        private void ShowListButton()
        {
            var sos = FindAllScriptableObjects<CharacterData_SO>("Assets/Game Data/Monster Data/");

            for (int i = 0; i < sos.Length; i++)
            {
                var b = new MonsterNameButton(sos[i]);
                // 注册点击事件，执行ShowCharacterData方法
                b.OnButtonClick = ShowCharacterData;
                // 设置按钮位置
                leftPanel.Add(b);
                // 存储对象
                monsterList.Add(b);
            }
        }

        //private EventCallback<ChangeEvent>

        private void ShowCharacterData(CharacterData_SO character, MonsterNameButton monsterNameButton)
        {
            currentMonster = monsterNameButton;

            idIntegerField.value = character.id;
            nameTextField.value = character.monsterName;
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
            nameTextField.UnregisterValueChangedCallback(NameTextFieldCallback);
            nameTextField.RegisterValueChangedCallback(NameTextFieldCallback);
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

        private void NameTextFieldCallback(ChangeEvent<string> evt)
        {
            currentMonster.text = evt.newValue;
            string assetPath = AssetDatabase.GetAssetPath(currentMonster.character);
            AssetDatabase.RenameAsset(assetPath, $"{evt.newValue} Data");
            AssetDatabase.SaveAssets();
        }

        private void CreateSkillView(SkillData_SO skill)
        {
            // 创建技能视图
            MonsterSkillView monsterSkillView = new MonsterSkillView(skill);
            monsterSkillView.DeleteSkillButton_onClick = () => {
                skillView.Remove(monsterSkillView);
                currentMonster.character.DeleteSkill(skill);
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
