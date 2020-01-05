using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private AudioSource _audio;
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        _audio.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length + .1f);
        SceneManager.LoadScene("Level1");
    }
}
