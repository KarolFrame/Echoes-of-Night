using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource[] altavoces;
    public UnityEngine.UI.Slider slider;

    AudioSource audio;

    public Volume volume;

    private void Awake()
    {
        volume.Load();
        slider.value = volume.volume;
        altavoces = FindObjectsOfType<AudioSource>();
        audio = altavoces[0];
        for (int i = 0; i < altavoces.Length; i++)
        {
            altavoces[i].volume = slider.value;
        }
    }

    public void ChangeVolumen()
    {
        for (int i = 0; i < altavoces.Length; i++)
        {
            altavoces[i].volume = slider.value;
        }
        volume.volume = slider.value;
        volume.Save();
    }

    public void PlayAudio(AudioSource s, int a)
    {
        if(a==2) 
        {
            a = Random.Range(2, 8);
        }

        s.PlayOneShot(clips[a]);
    }
    public void PlayAudioUI()
    {

        audio.PlayOneShot(clips[22]);
    }

}
