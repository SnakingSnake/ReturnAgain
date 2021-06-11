using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> createList = new List<GameObject>();

    public void Start()
    {
        int ranNum = Random.Range(0, createList.Count);
        Instantiate(createList[ranNum], this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform.parent.gameObject.transform.transform);
        Destroy(this.gameObject);
    }
}