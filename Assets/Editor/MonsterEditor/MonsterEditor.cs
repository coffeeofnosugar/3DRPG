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

            // ��Ӳ˵����¼�
            addButton.clicked += AddButton_onClick;
            // չʾ���й���İ�ť
            ShowListButton();
            // ��ȡ��һ�����ﰴť��չʾ������
            var button = root.Q<MonsterNameButton>("MonsterNameButton");
            button.ShowCharacterData();

            addSkillButton.clicked += () => {
                // ����AttackData_SO
                AttackData_SO so = currentMonster.CreateAttackData_SO();
                CreateSkillView(so);
            };
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
                // ע�����¼���ִ��ShowCharacterData����
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
            // ������ͼ
            MonsterSkillView monsterSkillView = new MonsterSkillView(so);
            skillView.Add(monsterSkillView);
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
