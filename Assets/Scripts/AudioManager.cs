using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public AudioSource[] _audios;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnStageUp += AddSource;
        GameManager.Instance.OnStageDown += RemoveSource;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStageUp -= AddSource;
        GameManager.Instance.OnStageDown -= RemoveSource;

    }

    public void AddSource(int stage)
    {
        if (stage >= _audios.Length)
            return;
        
        _audios[stage].mute = false;
    }
    
    public void RemoveSource(int stage)
    {
        Debug.Log("remove");
        if (stage < 0)
            return;
        
        _audios[stage].mute = true;
    }
    
}
