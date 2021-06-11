using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuButton : MonoBehaviour
{

    public void Click_New()
    {
        NextScene.LoadScene("FirstScene");
    }

    public void Click_Exit()
    {
        Application.Quit(); // 어플리케이션 종료
    }
}
