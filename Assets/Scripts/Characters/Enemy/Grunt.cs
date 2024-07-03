using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public class Grunt : FSM
    {

        public void Skill()
        {
            if (characterStats.attackTarget)
            {
                //transform.LookAt(characterStats.attackTarget.transform.position);

                Vector3 direction = (characterStats.attackTarget.transform.position - transform.position).normalized;

                var targetAgent = characterStats.attackTarget.GetComponent<NavMeshAgent>();
                //targetAgent.isStopped = true;
                targetAgent.velocity = direction * characterStats.KickForce;
            }
        }
    }
}