using System;
using DG.Tweening;
using Player;
using Sirenix.OdinInspector;
using Tools.CoffeeTools;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed;
    public float waitTime;
    [SerializeField, ReadOnly] private float time;
    [SerializeField, ReadOnly] public int i;
    [SerializeField] private Vector3[] movePos;
    [SerializeField, ReadOnly] private GameObject player;
    private CharacterController _characterController;

    private void Update()
    {
        if (Vector3.Distance(transform.parent.position, movePos[i]) <= .1f)
        {
            if (time > waitTime)
            {
                i = i == 0 ? 1 : 0;
                time = 0;
            }
            else
                time += Time.deltaTime;
        }
        else if (player)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, movePos[i], speed * Time.deltaTime);
            int j = i == 0 ? 1 : 0;
            _characterController.Move((movePos[i] - movePos[j]).normalized * (speed * Time.deltaTime));
        }
        else
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, movePos[i], speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            _characterController = player.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
