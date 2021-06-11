using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public List<GameObject> wallList = new List<GameObject>();
    public void Start()
    {
        int ranNum = Random.Range(0, wallList.Count);
        var wallUp = Instantiate(wallList[ranNum], this.gameObject.transform.position + new Vector3(0,2,0), this.gameObject.transform.rotation, this.gameObject.transform);
        ranNum = Random.Range(0, wallList.Count);
        var wallMiddle = Instantiate(wallList[ranNum], this.gameObject.transform.position + new Vector3(0, 1, 0), this.gameObject.transform.rotation, this.gameObject.transform);
        ranNum = Random.Range(0, wallList.Count);
        var wallDown = Instantiate(wallList[ranNum], this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
        wallUp.gameObject.isStatic = true;
        wallMiddle.gameObject.isStatic = true;
        wallDown.gameObject.isStatic = true;
    }
}
