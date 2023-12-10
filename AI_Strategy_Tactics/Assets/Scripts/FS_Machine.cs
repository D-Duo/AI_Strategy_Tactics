using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class FS_Machine : MonoBehaviour
{

    public NavMeshAgent agent;
    public NavMeshAgent police;
    public GameObject key;

    private float _stealDist = 2;

    [Header("Wander Settings")]

    [Range(0, 10)] public float WanderSpeed = 1f;
    [Range(1, 500)] public float walkRadius = 10f;
    [Range(0, 100)] public int maxRestingTime = 5;
    int restingTime = 1;
    bool firstWander = true;

    [Header("Approach Settings")]

    [Range(0, 100)] public float SafeDistance;
    [Range(0, 10)] public float ApproachSpeed = 5f;

    [Header("Hide Settings")]

    GameObject[] hidingSpots;
    [Range(0, 10)] public float HidingSpeed = 5f;

    [Header("Coroutine Settings")]

    private WaitForSeconds wait = new WaitForSeconds(0.05f);
    delegate IEnumerator State();
    private State state;

    IEnumerator Start()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("hide2");

        state = Wander;

        while (enabled)
        {
            yield return StartCoroutine(state());
        }
    }

    IEnumerator Wander()
    {
        Debug.Log("Wander state");
        agent.speed = WanderSpeed;

        while (Vector3.Distance(police.transform.position, key.transform.position) < SafeDistance)
        {
            WanderF();

            yield return wait;
        };

        state = Approach;
    }

    IEnumerator Approach()
    {
        Debug.Log("Approach state");

        agent.speed = ApproachSpeed;
        Seek(key.transform.position);

        bool stolen = false;
        while (Vector3.Distance(police.transform.position, key.transform.position) > SafeDistance)
        {
            if (Vector3.Distance(key.transform.position, transform.position) < _stealDist)
            {
                stolen = true;
                break;
            };
            yield return wait;
        };

        if (stolen)
        {
            Steal();
            agent.speed = HidingSpeed;
            state = Hide;
        }
        else
        {
            firstWander = true;
            agent.speed = WanderSpeed;
            state = Wander;
        }
    }

    IEnumerator Hide()
    {
        while (true)
        {
            HideF();

            yield return wait;
        }
    }

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame

    void Seek(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    void Steal()
    {
        foreach (Transform child in key.transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
        key.GetComponent<Renderer>().enabled = false;
        Debug.Log("Stolen");
    }

    void WanderF()
    {
        if(firstWander)
        {
            agent.ResetPath(); 
            firstWander = false;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 destination = Vector3.zero;
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius, 1))
            {
                destination = hit.position;
            }

            if (restingTime == 0)
            {
                Seek(destination);
                restingTime = UnityEngine.Random.Range(0, maxRestingTime);
            }
            else if (restingTime >= 0)
            {
                restingTime--;
            }
        }
    }

    void HideF()
    {
        Func<GameObject, float> distance = (hs) => Vector3.Distance(police.transform.position, hs.transform.position);
        GameObject hidingSpot = hidingSpots.Select(ho => (distance(ho), ho)).Min().Item2;
        Vector3 dir = hidingSpot.transform.position - police.transform.position;
        Ray backRay = new Ray(hidingSpot.transform.position, -dir.normalized);
        RaycastHit info;
        hidingSpot.GetComponent<Collider>().Raycast(backRay, out info, 50f);
        Seek(info.point + dir.normalized);
    }
}
