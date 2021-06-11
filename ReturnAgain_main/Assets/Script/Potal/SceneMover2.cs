using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover2 : MonoBehaviour
{
    public pocalcont portalController;

    private void OnTriggerEnter(Collider other)
    {

        SceneManager.LoadScene("ReturnAgain");

    }
}