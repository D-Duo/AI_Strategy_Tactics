using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    //float freq = 0f;
    public static FlockingManager myManager;
    public GameObject beePrefab;
    public GameObject LiderPrefab;
    public int numBee = 20;
    public GameObject[] allBees;
    public Vector3 hiveLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos = Vector3.zero;
    [Range(1, 100)] public int freqFlock = 10;
    public bool RandGoal = false;
    Vector3 randPos;

    //[Header("Lider Settings")]

    //public bool FollowLider = false;
    //int numLiders = 0;

    [Header("Bee Settings")]

    [Range(1, 10)] public float Max_Speed;
    [Range(1, 10)] public float Min_Speed;
    [Range(1, 10)] public float Neighbour_Dist;
    [Range(1, 10)] public float Rotation_Speed;



    // Start is called before the first frame update
    void Start()
    {
        allBees = new GameObject[numBee];
        for (int i = 0; i < numBee; ++i)
        {
            randPos = this.transform.position + new Vector3(Random.Range(-hiveLimits.x, hiveLimits.x),
                                                                Random.Range(-hiveLimits.y, hiveLimits.y),
                                                                Random.Range(-hiveLimits.z, hiveLimits.z));
            //Vector3 randomize = randPosBee;
            allBees[i] = (GameObject)Instantiate(beePrefab, randPos, Quaternion.identity);
        }
        myManager = this;
        goalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if(FollowLider && numLiders == 0)
        //{
        //    LiderPrefab = Instantiate(LiderPrefab, randPos, Quaternion.identity);
        //    LiderPrefab.tag = "Lider";
        //    numLiders++;
        //}
        
        //if(!FollowLider)
        //{
        //    LiderPrefab.SetActive(false);
        //}
        //else
        //{
        //    LiderPrefab.SetActive(true);
        //}
      
        
        if (Random.Range(0, 100) < 1 && RandGoal)
        {
            goalPos = this.transform.position + new Vector3(Random.Range(-hiveLimits.x, hiveLimits.x),
                                                            Random.Range(-hiveLimits.y, hiveLimits.y),
                                                            Random.Range(-hiveLimits.z, hiveLimits.z));
        }
    }
}
