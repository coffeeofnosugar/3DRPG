using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SetAnimatorParametersAsset : PlayableAsset
{
    private SetAnimatorParametersBehaviour _behaviour = new SetAnimatorParametersBehaviour();

    [SerializeField, EnumToggleButtons, HideLabel] private AnimatorControllerParameterType type;
    [SerializeField] private string parametersName;

    [SerializeField, ShowIf("type", AnimatorControllerParameterType.Bool)]
    private bool _bool;

    [SerializeField, ShowIf("type", AnimatorControllerParameterType.Int)]
    private int _int;
    
    [SerializeField, ShowIf("type", AnimatorControllerParameterType.Float)]
    private int _float;
    
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        return Playable.Create(graph);
    }
}
