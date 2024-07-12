using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Weight : CompositeNode
    {
        public List<int> weights = new List<int>();

        private int selectIndex;

        protected override void OnStart()
        {
            int totalWeight = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                totalWeight += weights[i];
            }

            int randomValue = Random.Range(0, totalWeight);

            int cumulativeWeight = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                cumulativeWeight += weights[i];
                if (randomValue < cumulativeWeight)
                {
                    selectIndex = i;
                    break;
                }
            }
            Debug.Log($"Ö´ÐÐ{selectIndex}");
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return children[selectIndex].Update();
        }
    }
}