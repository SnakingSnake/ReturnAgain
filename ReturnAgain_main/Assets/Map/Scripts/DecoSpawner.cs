using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoSpawner : MonoBehaviour
{
    public List<GameObject> decocreateList = new List<GameObject>();

    public void Start()
    {
        int ranNum = Random.Range(0, decocreateList.Count);
        var decocreate = Instantiate(decocreateList[ranNum], this.gameObject.transform.position, Quaternion.Euler(0, Random.Range(0, 4)*90-45, 0), this.gameObject.transform.parent.gameObject.transform.transform);
        decocreate.gameObject.isStatic = true;
        Destroy(this.gameObject);
    }
}
