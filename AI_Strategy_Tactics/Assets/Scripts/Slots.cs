using UnityEngine;

public class Slots : MonoBehaviour
{
    public int NumberOfRows;
    public int NumberOfPrefabs;
    public GameObject Prefab;
    public GameObject Leader;

    void Start()
    {
        float pos = -2f;
        for (int i = 0; i < NumberOfRows; i++) 
        {
            CreateRow(NumberOfPrefabs, pos, Prefab);
            pos -= 2f;
        }
    }

    void CreateRow(int num, float z, GameObject pf)
    {
        float pos = 1 - num;
        for (int i = 0; i < num; ++i)
        {
            Vector3 position = Leader.transform.TransformPoint(new Vector3(pos, 0f, z));
            GameObject temp = Instantiate(pf, position, Leader.transform.rotation);
            temp.AddComponent<Formation>();
            temp.GetComponent<Formation>().pos = new Vector3(pos, 0, z);
            temp.GetComponent<Formation>().target = Leader;
            pos += 2f;
        }
    }
}
