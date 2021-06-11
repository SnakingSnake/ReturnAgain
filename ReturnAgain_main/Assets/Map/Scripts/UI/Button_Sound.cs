using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Sound : MonoBehaviour
{
    private AudioSource musicPlayer;
    public AudioClip Sound;
    public void Btn_Click_sound()
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = Sound;
        musicPlayer.loop = false;
        musicPlayer.mute = false;
        musicPlayer.playOnAwake = false;
        musicPlayer.Play();

    }
}
