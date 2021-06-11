using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalCont : MonoBehaviour
{
    public Transform land1, land2;
    public Transform playerRoot, playerCam;
    public Transform portalCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Teleport()
    {
        var playerland = land1;
        land1 = land2 ;
        land2 = playerland;
        playerRoot.position = portalCam.position;
    }
}
