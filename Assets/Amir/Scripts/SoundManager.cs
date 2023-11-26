using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundManager
{
    [SerializeField] AudioSource audioHit1;
    [SerializeField] AudioSource audioHit2;
    [SerializeField] AudioSource audioDeath;
    [SerializeField] AudioSource audioLaunch;
    [SerializeField] AudioSource audioCollide1;
    [SerializeField] AudioSource audioCollide2;
    [SerializeField] float soundRate;

    float _soundRate1 = 1, _soundRate2 = 1;

    public void PlayHitSound()
    {
        if (_soundRate1 >= soundRate)
        {
            if (audioHit1.isPlaying)
                audioHit2.Play();
            else
                audioHit1.Play();

            _soundRate1 = 0;
        }
    }

    public void PlayCollideSound()
    {
        if (_soundRate2 >= soundRate)
        {
            if (audioCollide1.isPlaying)
                audioCollide2.Play();
            else
                audioCollide1.Play();

            _soundRate2 = 0;
        }
    }

    public void PlayDeathSound()
    {
        audioDeath.Play();
    }

    public void PlayLaunchSound()
    {
        audioLaunch.Play();
    }

    public void Update()
    {
        if (_soundRate1 < soundRate)
            _soundRate1 += Time.deltaTime;

        if (_soundRate2 < soundRate)
            _soundRate2 += Time.deltaTime;
    }
}
