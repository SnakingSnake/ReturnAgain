using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagePortal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NextScene.LoadScene("DungeonScene");
        }
    }
}
