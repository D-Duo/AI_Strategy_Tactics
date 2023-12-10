using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_mov_Hide : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    GameObject[] hidingSpots;
    //hidingSpots = GameObject.FindGameObjectsWithTag("hide");

    float freq = 0f;

    // Update is called once per frame
    void Update()
    {
        freq += Time.deltaTime;
        if (freq > 0.5)
        {
            freq -= 0.5f;
            Hide();
        }
    }

    void Hide()
    {
    //    Func<GameObject, float> distance =
    //        (hs) => Vector3.Distance(target.transform.position,
    //                                 hs.transform.position);
    //    GameObject hidingSpot = hidingSpots.Select(
    //        ho => (distance(ho), ho)
    //        ).Min().Item2;
    //    Vector3 dir = hidingSpot.transform.position - target.transform.position;
    //    Ray backRay = new Ray(hidingSpot.transform.position, -dir.normalized);
    //    RaycastHit info;
    //    hidingSpot.GetComponent<Collider>().Raycast(backRay, out info, 50f);

    //    agent.destination = (info.point + dir.normalized);
    }
}
