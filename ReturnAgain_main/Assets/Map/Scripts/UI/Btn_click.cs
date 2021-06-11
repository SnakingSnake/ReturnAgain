using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_click : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Btn_off();
        }
        
    }
    public void Btn_Click()
    {
        this.gameObject.SetActive(true);
    }

    public void Btn_off()
    {
        this.gameObject.SetActive(false);
    }
}
