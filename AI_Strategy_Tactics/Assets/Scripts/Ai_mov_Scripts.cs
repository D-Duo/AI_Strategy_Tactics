using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_mov_Scripts : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;

    public string movScript = "none";
    [Range(0, 100)] public float speed = 5f;
    [Range(1, 500)] public float walkRadius = 10f;
    [Range(0, 100)] public int maxRestingTime = 5;
    public bool humanBehaviour = true;

    public string waypointsTag = "none";
    public GameObject[] waypoints;
    int patrolWP = 0;
    int direction = 0;

    float freq = 0f;
    int restingTime = 1;

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
        if(agent == null) { return; }

        if (!humanBehaviour)
        {
            freq += Time.deltaTime;
            if (freq > 0.5)
            {
                freq -= 0.5f;
                switch (movScript)
                {
                    case "SeekTarget":
                        SeekTarget();
                        break;

                    case "Wander":
                        Wander();
                        break;

                    case "Patrol":
                        Patrol();
                        break;

                    case "GhostPatrol":
                        GhostPatrol();
                        break;

                    default:
                        break;
                }
            }
        }
        else
        {
            switch (movScript)
            {
                case "SeekTarget":
                    SeekTarget();
                    break;

                case "Wander":
                    Wander();
                    break;

                case "Patrol":
                    Patrol();
                    break;

                case "GhostPatrol":
                    GhostPatrol();
                    break;

                default:
                    break;
            }
        }
    }

    void SeekTarget()
    {
        agent.SetDestination(target.transform.position);
    }

    void Seek(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    void Wander()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 destination = Vector3.zero;
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius, 1))
            {
                destination = hit.position;
            }

            if (restingTime == 0)
            {
                Seek(destination);
                restingTime = Random.Range(0, maxRestingTime);
            }
            else if (restingTime >= 0)
            {
                restingTime--;
            }
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5)
        {
            Seek(waypoints[patrolWP].transform.position);
            if (direction == 0) { patrolWP = (patrolWP + 1) % waypoints.Length; }
            else { patrolWP = (patrolWP - 1) % waypoints.Length; }

            if(patrolWP < 0) { patrolWP = waypoints.Length - 1; }
        }
    }

    void GhostPatrol()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if(distance <= 8) {
            if (agent.isStopped) { agent.isStopped = false; }
            else { Patrol(); }
        }
        else { agent.isStopped = true; }        
    }
}
