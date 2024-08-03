using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// ˲�伤�����壬ֻ�����ó�һ֡����
/// </summary>
[System.Serializable]
public class ActivetionGameObjectAsset : PlayableAsset
{
    private ActivetionGameObjectBehaviour _behaviour = new ActivetionGameObjectBehaviour();

    [SerializeField] private List<GameObject> GameObjects;
    
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<ActivetionGameObjectBehaviour>.Create(graph, _behaviour);
        
        var playableBehaviour = playable.GetBehaviour();
        
        playableBehaviour.GameObjects = GameObjects;
        return playable;
    }
}
