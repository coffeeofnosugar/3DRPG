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

            // ��Ӳ˵����¼�
            addButton = root.Q<Button>("AddToolbarButton");
            addButton.clicked += AddButton_onClick;

            deleteButton = root.Q<Button>("DeleteToolbarButton");
            deleteButton.clicked += DeleteButton_onClick;

            addSkillButton = root.Q<Button>("AddSkillButton");
            addSkillButton.clicked += () => {
                // ����SkillData_SO
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

            // չʾ���й���İ�ť
            ShowListButton();
            // ��ȡ��һ�����ﰴť��չʾ������
            monsterList[0].ShowCharacterData();
        }

        /// <summary>
        /// ��ӹ���
        /// </summary>
        private void AddButton_onClick()
        {
            var character = ScriptableObject.CreateInstance<CharacterData_SO>();
            AssetDatabase.CreateAsset(character, $"Assets/Game Data/Monster Data/{character.monsterName} Data.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            var b = new MonsterNameButton(character);
            // ע�����¼���ִ��ShowCharacterData����
            b.OnButtonClick = ShowCharacterData;
            // ���ð�ťλ��
            leftPanel.Add(b);
            // �洢����
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

            // ��ȡ��һ�����ﰴť��չʾ������
            monsterList[0].ShowCharacterData();
        }

        /// <summary>
        /// չʾ���й���
        /// </summary>
        private void ShowListButton()
        {
            var sos = FindAllScriptableObjects<CharacterData_SO>("Assets/Game Data/Monster Data/");

            for (int i = 0; i < sos.Length; i++)
            {
                var b = new MonsterNameButton(sos[i]);
                // ע�����¼���ִ��ShowCharacterData����
                b.OnButtonClick = ShowCharacterData;
                // ���ð�ťλ��
                leftPanel.Add(b);
                // �洢����
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
            // ����������ͼ
            MonsterSkillView monsterSkillView = new MonsterSkillView(skill);
            monsterSkillView.DeleteSkillButton_onClick = () => {
                skillView.Remove(monsterSkillView);
                currentMonster.character.DeleteSkill(skill);
            };
            skillView.Add(monsterSkillView);
        }

        /// <summary>
        /// Ѱ��ָ��·��������ָ��ScriptableObject����ļ�
        /// </summary>c
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
