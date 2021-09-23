using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option_Menu : MonoBehaviour
{
    public Slider BGM;
    public Slider FX;

    // Update is called once per frame
    void Update()
    {
        Player_knights.BGM_Set = (int)BGM.value;
        Player_knights.FX_Set = (int)FX.value;
    }
}
