using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            DontDestroyOnLoad(instance);
        }

    }


    public void SFXPlay(string sfxName, AudioClip clip, bool skill = false, float skillTime = 0f)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        if (skill)
        {
            audiosource.loop = true;
            audiosource.Play();

            if (skillTime == 0f)
            {
                Destroy(go, clip.length);
            }
            else
            {
                Debug.Log("실행됐어요");
                Destroy(go, skillTime);
            }
        }
        else
        {
            
            audiosource.clip = clip;
            if (sfxName.Equals("Walk1") || sfxName.Equals("Walk2"))
            {
                audiosource.volume = 0.25f;
            }
            audiosource.Play();

            Destroy(go, clip.length);
        }
    }        
}
