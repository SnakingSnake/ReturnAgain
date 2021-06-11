using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fllow : MonoBehaviour
{
    public Vector3 offset;
    Transform target;
    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        target = player.transform;
    }

    void Update()
    {
        transform.position = target.position + offset; //카메라 시점고정
    }
}
