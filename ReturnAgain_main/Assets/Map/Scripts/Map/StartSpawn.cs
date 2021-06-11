using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpawn : MonoBehaviour
{
    public List<GameObject> Portals = new List<GameObject>();
    public void Start()
    {
        int ranNum = Random.Range(0, Portals.Count);
        var portal = Instantiate(Portals[ranNum], this.gameObject.transform.position + new Vector3(0,0.03f,0) , Quaternion.Euler(0, Random.Range(0, 4) * 90 - 45, 0), this.gameObject.transform);
        portal.SetActive(true);
    }
}
