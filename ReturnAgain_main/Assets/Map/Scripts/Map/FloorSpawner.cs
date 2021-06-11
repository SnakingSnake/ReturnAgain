using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public List<GameObject> floorList = new List<GameObject>();

    public void Start()
    {
        int ranNum = Random.Range(0, floorList.Count);
        var floor = Instantiate(floorList[ranNum], this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform.parent.gameObject.transform.transform);
        floor.gameObject.isStatic = true;
        Destroy(this.gameObject);
    }
}
