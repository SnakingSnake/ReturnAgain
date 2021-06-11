using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMain : MonoBehaviour
{
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    public void BacktoMain_Btn()
    {
        SceneManager.LoadScene("MainScene");
    }
}
