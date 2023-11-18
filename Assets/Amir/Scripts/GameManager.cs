using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;



    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
        }
    }
    #endregion



    public SoundManager SoundManager => _soundManager;

    

    private void Update()
    {
        SoundManager.Update();
    }
}

[Serializable]
public class SoundManager
{
    [SerializeField] AudioSource audioHit1;
    [SerializeField] AudioSource audioHit2;
    [SerializeField] AudioSource audioDeath;
    [SerializeField] AudioSource audioCollide;
    [SerializeField] float soundRate;

    float _soundRate;

    public void PlayCollision()
    {
        if (_soundRate >= soundRate)
        {
            if (audioHit1.isPlaying)
                audioHit2.Play();
            else
                audioHit1.Play();

            _soundRate = 0;
        }
    }

    public void PlayDeath()
    {
        audioDeath.Play();
    }

    public void Update()
    {
        _soundRate += Time.deltaTime;
    }
}
