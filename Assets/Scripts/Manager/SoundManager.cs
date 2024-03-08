using System;
using System.Collections;
using System.Collections.Generic;
using Consts;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private BgmAudioClipDictionary dicBgmClip;
    [SerializeField] private SfxAudioClipDictionary dicSfxClip;
    
    // opening, main, gameover
    public static SoundManager Inst { get; private set; }

    private AudioSource audioSrc;
    private float sfxVolume = 1.0f;
    
    public Slider sfxSlider;
    
    private void Awake()
    {
        if (Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        //audioSrc.clip = dicBgmClip[eBgm.Opening];

        sfxVolume = PlayerPrefs.GetFloat("SoundFx");
        if(sfxVolume == 0.0f) 
            sfxVolume = 1.0f;
        sfxSlider.value = sfxVolume;
    }

    public void SetBgm(eBgm eBgm)
    {
        audioSrc.Stop();
        audioSrc.clip = dicBgmClip[eBgm];
        audioSrc.Play();
    }

    //public void SetBgmVolume(float volum)
    //{
    //    _audioSource.volume = volum;
    //}

    public void SetSfxVolume(float volum)
    {
        sfxVolume = volum;
        Debug.Log(sfxVolume);
        PlayerPrefs.SetFloat("SoundFx", sfxVolume);
    }
    
    public void PlayButtonSfx() => SfxPlay(eSfx.Button);

    public void SfxPlay(eSfx sfx)
    {
        GameObject go = new GameObject(sfx + " Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = dicSfxClip[sfx];
        audioSource.volume = sfxVolume;
        audioSource.Play();
        
        Destroy(go, audioSource.clip.length);
    }
}
