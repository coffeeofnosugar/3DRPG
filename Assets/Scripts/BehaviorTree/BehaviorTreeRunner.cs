using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        BehaviorTree tree;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviorTree>();

            var loop = ScriptableObject.CreateInstance<RepeatNode>();

            var sequ = ScriptableObject.CreateInstance<SequencerNode>();

            var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
            log1.startMessage = "log1";
            log1.updateMessage = "log1";
            log1.stopMessage = "log1";

            var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
            log2.startMessage = "log2";
            log2.updateMessage = "log2";
            log2.stopMessage = "log2";

            var wait = ScriptableObject.CreateInstance<WaitNode>();
            wait.duration = 5;

            tree.rootNode = loop;
            loop.child = sequ;
            sequ.children.Add(log1);
            sequ.children.Add(log2);
            sequ.children.Add(wait);
        }

        private void Update()
        {
            tree.Update();
        }
    }
}