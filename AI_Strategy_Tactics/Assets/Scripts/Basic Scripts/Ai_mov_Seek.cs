using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_mov_Seek : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject target;

    float freq = 0f;

    // Update is called once per frame
    void Update()
    {
        freq += Time.deltaTime;
        if (freq > 0.5)
        {
            freq -= 0.5f;
            AI_Scripts.Mov_Scripts.SeekTarget(agent, target.transform);
        }
    }
}