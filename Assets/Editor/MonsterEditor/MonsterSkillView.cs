using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace MonsterEditor
{
    public class MonsterSkillView : UnityEngine.UIElements.VisualElement
    {
        public System.Action DeleteSkillButton_onClick;
        
        Foldout foldout;

        TextField skillNameTextField, animationTextField;
        FloatField rangeFloatFied, coolDownFloatFied;
        IntegerField minDamge, maxDamge;
        FloatField kickForceFloatField;
        Button deleteSkillButton;

        public MonsterSkillView(SkillData_SO skill)
        {
            // ���� UXML �ļ�
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/MonsterEditor/MonsterSkillView.uxml");
            VisualElement root = visualTree.CloneTree();

            // �����ص�������ӵ���ǰԪ��
            this.Add(root);
            string assetPath = AssetDatabase.GetAssetPath(skill);
            // ��ȡ���
            foldout = this.Q<Foldout>("Foldout");
            skillNameTextField = this.Q<TextField>("SkillNameTextField");
            skillNameTextField.bindingPath = "skillName";
            animationTextField = this.Q<TextField>("AnimationTextField");
            animationTextField.bindingPath = "animation";
            rangeFloatFied = this.Q<FloatField>("AttackRangeFloatField");
            rangeFloatFied.bindingPath = "attackRange";
            coolDownFloatFied = this.Q<FloatField>("CoolDownFloatField");
            coolDownFloatFied.bindingPath = "coolDown";
            minDamge = this.Q<IntegerField>("MinDamgeIntegerField");
            minDamge.bindingPath = "minDamge";
            maxDamge = this.Q<IntegerField>("MaxDamgeIntegerField");
            maxDamge.bindingPath = "maxDamge";
            kickForceFloatField = this.Q<FloatField>("KickForceFloatField");
            kickForceFloatField.bindingPath = "kickForce";

            deleteSkillButton = this.Q<Button>("DeleteSkillButton");

            deleteSkillButton.clicked += () => DeleteSkillButton_onClick?.Invoke();


            SerializedObject so = new SerializedObject(skill);
            skillNameTextField.Bind(so);
            // ��skillNameTextField����ʱͬ������foldout
            skillNameTextField.RegisterValueChangedCallback(evt =>
            {
                skill.name = evt.newValue;
                AssetDatabase.SaveAssets();
                foldout.text = evt.newValue;
            });
            animationTextField.Bind(so);
            rangeFloatFied.Bind(so);
            coolDownFloatFied.Bind(so);
            minDamge.Bind(so);
            maxDamge.Bind(so);
            kickForceFloatField.Bind(so);
        }
    }
}