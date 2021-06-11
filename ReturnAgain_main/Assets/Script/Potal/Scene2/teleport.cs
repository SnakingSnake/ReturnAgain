using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public PotalCont PortalController;
    private void OnTriggerEnter(Collider other)
    {
        PortalController.Teleport();
    }
}