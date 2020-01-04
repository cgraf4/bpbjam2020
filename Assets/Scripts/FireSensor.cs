using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSensor : MonoBehaviour
{
    private PlayerController _player;
    public GameObject particlePrefab;
    private void OnEnable()
    {
        InputManager.Instance.OnInteractionKeyPressed += SpawnParticle;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInteractionKeyPressed -= SpawnParticle;

    }

    private void SpawnParticle()
    {
        Instantiate(particlePrefab, transform);
    }
}
