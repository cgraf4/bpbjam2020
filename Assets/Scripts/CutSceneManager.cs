using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    private Animator _anim;
    private PlayerController _player;
    public Transform[] startFires;
    
    private IEnumerator Start()
    {
        _anim = GetComponent<Animator>();
        _player = FindObjectOfType<PlayerController>();
        
        yield return new WaitForSeconds(.9f);
        GameManager.Instance.SpawnFire(startFires[0].position);
        yield return new WaitForSeconds(.9f);
        GameManager.Instance.SpawnFire(startFires[1].position);
        yield return new WaitForSeconds(.9f);
        GameManager.Instance.SpawnFire(startFires[2].position);
    }

    public void LetPlayerMove()
    {
//        Debug.Log("let player move");
        _player.canMove = true;
    }
}
