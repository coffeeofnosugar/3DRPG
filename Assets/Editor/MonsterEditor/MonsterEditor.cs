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

            // ��Ӳ˵����¼�
            addButton.clicked += AddButton_onClick;
            // չʾ���й���İ�ť
            ShowListButton();
            // ��ȡ��һ�����ﰴť��չʾ������
            var button = root.Q<MonsterNameButton>("MonsterNameButton");
            button.ShowCharacterData();

            addSkillButton.clicked += () => {
                // ����AttackData_SO
                SkillData_SO skill = currentMonster.CreateSkill();
                CreateSkillView(skill);
            };
        }

        /// <summary>
        /// ��ӹ���
        /// </summary>
        private void AddButton_onClick()
        {
            var character = ScriptableObject.CreateInstance<CharacterData_SO>();
            var b = new MonsterNameButton(character);
            leftPanel.Add(b);
            // ����
            AssetDatabase.CreateAsset(character, $"Assets/Game Data/Charater Data/{character.monsterName} Data.asset");
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
                // ע�����¼���ִ��ShowCharacterData����
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
            // ����������ͼ
            MonsterSkillView monsterSkillView = new MonsterSkillView(skill);
            monsterSkillView.DeleteSkillButton_onClick = () => {
                skillView.Remove(monsterSkillView);
                currentMonster.DeleteSkill(skill);
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
