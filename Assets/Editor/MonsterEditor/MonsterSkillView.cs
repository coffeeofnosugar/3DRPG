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
            // 加载 UXML 文件
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/MonsterEditor/MonsterSkillView.uxml");
            VisualElement root = visualTree.CloneTree();

            // 将加载的内容添加到当前元素
            this.Add(root);
            string assetPath = AssetDatabase.GetAssetPath(skill);
            // 获取组件
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
            // 当skillNameTextField更改时同步更改foldout
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