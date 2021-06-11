using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMove : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
        Destroy(gameObject, 3f);
    }
}
