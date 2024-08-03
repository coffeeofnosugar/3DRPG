using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 瞬间激活物体，只需设置成一帧即可
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
