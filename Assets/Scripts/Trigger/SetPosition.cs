using System.Collections;
using System.Collections.Generic;
using Tools.CoffeeTools;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    [SerializeField] private Transform targetPos;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debugs.Show("´¥·¢´«ËÍ", Debugs.DebugTypeEnum.Normal);
            other.transform.position = targetPos.position;
            other.transform.rotation = targetPos.rotation;
            Debug.Log(other.transform.position);
        }
    }
}
