using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class ActivetionGameObjectBehaviour : PlayableBehaviour
{
    public List<GameObject> GameObjects;
    
    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (GameObjects == null)
            return;
        foreach (var o in GameObjects.Where(o => o != null))
        {
            o.SetActive(!o.activeSelf);
        }
    }
}
