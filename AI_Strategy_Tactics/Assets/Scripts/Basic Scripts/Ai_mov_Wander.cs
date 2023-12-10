using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_mov_Wander : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    [Range(0, 100)] public float speed = 5f;
    [Range(1, 500)] public float walkRadius = 10f;
    [Range(0, 100)] public int maxRestingTime = 5;

    float freq = 0f;
    int restingTime = 0;

    void Start()
    {
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        freq += Time.deltaTime;
        if (freq > 0.5)
        {
            freq -= 0.5f;
            if (agent != null) { StartCoroutine(AI_Scripts.Mov_Scripts.Wander(agent, walkRadius, 1, restingTime)); }
        }
    }
}
