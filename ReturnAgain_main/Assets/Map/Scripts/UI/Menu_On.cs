using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_On : MonoBehaviour
{
    public GameObject Menu;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Player_knights.activeInven == false)
            {
                if (Menu.activeSelf == false)
                {
                    Menu.SetActive(true);
                }

            }
        }
    }
}
