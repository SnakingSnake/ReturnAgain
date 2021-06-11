using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject start = GameObject.FindWithTag("Start");
        Vector3 vec = start.gameObject.transform.position;
        player.gameObject.transform.position = vec;
    }
}
