using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    private AudioSource musicPlayer;
    public AudioClip Sound;
    void Boss_Clear()
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = Sound;
        musicPlayer.loop = false;
        musicPlayer.mute = false;
        musicPlayer.playOnAwake = false;
        musicPlayer.Play();
        Destroy(GameObject.FindWithTag("BossDoor"));
    }
}
