using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterEditor
{
    public class MonsterSkillView : UnityEngine.UIElements.VisualElement
    {
        TextField animationText;
        FloatField rangeFloatFied, coolDownFloatFied;
        IntegerField minDamge, maxDamge;
        FloatField kickForceFloatField;
        Button deleteSkillButton;

        AttackData_SO so;
        public MonsterSkillView(AttackData_SO so)
        {
            // ���� UXML �ļ�
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/MonsterEditor/MonsterSkillView.uxml");
            VisualElement root = visualTree.CloneTree();

            // �����ص�������ӵ���ǰԪ��
            this.Add(root);

            this.so = so;

            // ��ȡ���
            animationText = this.Q<TextField>("AnimationTextField");
            rangeFloatFied = this.Q<FloatField>("RangeFloatFied");
            coolDownFloatFied = this.Q<FloatField>("CoolDownFloatFied");
            minDamge = this.Q<IntegerField>("MinDamge");
            maxDamge = this.Q<IntegerField>("MaxDamge");
            kickForceFloatField = this.Q<FloatField>("KickForceFloatField");
            deleteSkillButton = this.Q<Button>("DeleteSkillButton");
        }

        public void DeleteSkill()
        {

        }
    }
}