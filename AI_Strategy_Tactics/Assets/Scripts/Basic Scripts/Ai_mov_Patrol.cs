using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Ai_mov_Patrol : MonoBehaviour
{

    public NavMeshAgent agent;
    public GameObject target;

    [Range(0, 100)] public float speed = 5f;

    public string waypointsTag = "none";
    public GameObject[] waypoints;
    int patrolWP = 0;
    int direction = 0;
    public bool humanBehaviour = true;
    public bool ghostPatrol = false;

    float freq = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length == 0)
        {
            if (waypointsTag != "none")
            {
                waypoints = GameObject.FindGameObjectsWithTag(waypointsTag);

                if (waypoints.Length == 0)
                {
                    Debug.Log("No GameObjects are tagged with 'Waypoints1'");
                }
            }
            else
            {
                Debug.Log("No Tags where specified");
            }
        }

        agent.speed = speed;
        patrolWP = Random.Range(0, waypoints.Length);
        direction = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!humanBehaviour)
        {
            freq += Time.deltaTime;
            if (freq > 0.5)
            {
                freq -= 0.5f;
                if (agent != null)
                {
                    if(ghostPatrol) { GhostPatrol(); }
                    else { Patrol(); }
                }
            }
        }
        else
        {
            if (ghostPatrol) { GhostPatrol(); }
            else { Patrol(); }
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5)
        {
            agent.SetDestination(waypoints[patrolWP].transform.position);
            if (direction == 0) { patrolWP = (patrolWP + 1) % waypoints.Length; }
            else { patrolWP = (patrolWP - 1) % waypoints.Length; }

            if (patrolWP < 0) { patrolWP = waypoints.Length - 1; }
        }
    }

    void GhostPatrol()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= 8)
        {
            if (agent.isStopped) { agent.isStopped = false; }
            else { Patrol(); }
        }
        else { agent.isStopped = true; }
    }
}
