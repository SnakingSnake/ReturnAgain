using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNext : MonoBehaviour
{

    // Update is called once per frame
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject start = GameObject.FindWithTag("Start");
        Vector3 vec = start.gameObject.transform.position;
        player.gameObject.transform.position = vec;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            NextDungeon.Already = true;
            if (Player_knights.dungeonLevel < 4)
            {
                Player_knights.dungeonLevel++;
                NextScene.LoadScene("VillageScene");
            }
            else
            {
                NextScene.LoadScene("Boss1Scene");
            }
        }
    }
}
