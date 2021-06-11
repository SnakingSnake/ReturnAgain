using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pocalcont : MonoBehaviour
{
    public Transform boss, home;
    public Transform Playercamera, Character;
    public Transform Camera;
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
        var playerland = boss;
        boss = home;
        home = playerland;
        Character.position = Camera.position;
    }
}
