using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextDungeon : MonoBehaviour
{
    public static bool Already;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject start = GameObject.FindWithTag("Start");
        Vector3 vec = start.gameObject.transform.position;
        player.gameObject.transform.position = vec;
        if (Already == true)
        {
            Debug.Log("이미 있다.");
            Destroy(this.gameObject);
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        DontDestroyOnLoad(this.gameObject);
    }

}
