using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform tf_Crosshair;
   
 

    // Update is called once per frame
    void Update()
    {
        CrosshairMoving();
    } 
    void CrosshairMoving()
    {
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x - (Screen.width /2) , Input.mousePosition.y - (Screen.height /2));
    }
}
