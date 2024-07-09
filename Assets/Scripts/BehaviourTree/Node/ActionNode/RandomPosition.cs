using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class RandomPosition : ActionNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (blackboard.target)
            {
                return State.Failure;
            }

            float randomX = Random.Range(-characterStats.PatrolRange, characterStats.PatrolRange);
            float randomZ = Random.Range(-characterStats.PatrolRange, characterStats.PatrolRange);

            Vector3 randomPoint = new Vector3(characterStats.originPosition.x + randomX, characterStats.transform.position.y, characterStats.originPosition.z + randomZ);

            // NavMesh.SamplePosition�ú����Ƿ�ֹȡ�ĵ�û�б�mesh���ǣ�Ȼ��ȡ������ĵ�
            blackboard.moveToPosition = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1, 1) ? hit.position : characterStats.transform.position;
            return State.Success;
        }
    }
}