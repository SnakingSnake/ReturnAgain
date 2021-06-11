using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potalteleport : MonoBehaviour
{
    public pocalcont portalController;
    private void OnTriggerEnter(Collider other)
    {
        portalController.Teleport();
    }
}
