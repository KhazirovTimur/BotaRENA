using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public float searchPathDelay = 1.0f;
    private Transform player;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<FirstPersonController>().transform;
        timer += searchPathDelay;
    }



    public void MoveToPlayer()
    {
        agent.destination = player.position;
    }

    public bool CanCheckPlayerPos()
    {
        timer -= Time.deltaTime;
        if (timer <= Time.time)
        {
            timer = Time.time + searchPathDelay;
            return true;
        }
        return false;
    }

}