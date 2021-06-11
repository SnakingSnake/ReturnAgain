using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagePortal_boss : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NextScene.LoadScene("Boss1Scene");
        }
    }
}
