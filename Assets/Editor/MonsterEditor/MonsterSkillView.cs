using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterEditor
{
    public class MonsterSkillView : UnityEngine.UIElements.VisualElement
    {
        public System.Action DeleteSkillButton_onClick;

        TextField animationText;
        FloatField rangeFloatFied, coolDownFloatFied;
        IntegerField minDamge, maxDamge;
        FloatField kickForceFloatField;
        Button deleteSkillButton;

        SkillData_SO skill;
        public MonsterSkillView(SkillData_SO skill)
        {
            // ���� UXML �ļ�
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/MonsterEditor/MonsterSkillView.uxml");
            VisualElement root = visualTree.CloneTree();

            // �����ص�������ӵ���ǰԪ��
            this.Add(root);

            this.skill = skill;

            // ��ȡ���
            animationText = this.Q<TextField>("AnimationTextField");
            rangeFloatFied = this.Q<FloatField>("RangeFloatFied");
            coolDownFloatFied = this.Q<FloatField>("CoolDownFloatFied");
            minDamge = this.Q<IntegerField>("MinDamge");
            maxDamge = this.Q<IntegerField>("MaxDamge");
            kickForceFloatField = this.Q<FloatField>("KickForceFloatField");
            deleteSkillButton = this.Q<Button>("DeleteSkillButton");

            deleteSkillButton.clicked += () => DeleteSkillButton_onClick?.Invoke();
        }
    }
}