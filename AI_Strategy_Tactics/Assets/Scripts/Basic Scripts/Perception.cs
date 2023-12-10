using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Perception : MonoBehaviour
{
    private Transform target;

    public float wanderRadius = 10f;
    public float wanderTimer = 0f;

    public int numAgents = 5;
    public float spawnRadius = 2.0f;
    public float neighborRadius = 2.0f;

    private bool isSpoted = false;

    public Camera frustum;
    public LayerMask mask;

    private NavMeshAgent agent;
    private RaycastHit hit;

    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        agent = GetComponent<NavMeshAgent>();

        StateColor();

        StartCoroutine(PerceptionStart());
    }

    IEnumerator PerceptionStart()
    {
        while (enabled)
            yield return PerceptionStates();
    }

    IEnumerator PerceptionStates()
    {
        while (!isSpoted)
        {
            if(AI_Scripts.Detection_Scripts.FindInCameraFructum("Human", this.gameObject, frustum, mask, out hit))
            {
                this.transform.parent.BroadcastMessage("Spoted", hit.collider.gameObject.transform);
            }
            yield return AI_Scripts.Mov_Scripts.Wander(agent, wanderRadius, 1, 0);
        }

        AI_Scripts.Mov_Scripts.SeekTarget(agent, target);
    }

    void Spoted(Transform objective)
    {
        target = objective;
        isSpoted = true;
        StateColor();
    }

    void StateColor()
    {
        if (!isSpoted)
        {
            rend.material.SetColor("_Color", Color.green);
        }
        else
        {
            rend.material.SetColor("_Color", Color.red);
        }
    }
}
