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
            // 加载 UXML 文件
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/MonsterEditor/MonsterSkillView.uxml");
            VisualElement root = visualTree.CloneTree();

            // 将加载的内容添加到当前元素
            this.Add(root);

            this.so = so;

            // 获取组件
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