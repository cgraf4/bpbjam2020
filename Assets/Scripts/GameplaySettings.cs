﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gameplay Settings", menuName = "Settings/Gameplay Settings", order = 1)]

public class GameplaySettings : ScriptableObject
{
    [SerializeField] private int roundsUntilNewFire = 5;
    [SerializeField] private int maxLife = 3;
    [SerializeField] private float flickerInterval = .1f;
    [SerializeField] private int[] stages;

    public int[] Stages
    {
        get => stages;
    }
    public int RoundsUntilNewFire
    {
        get => roundsUntilNewFire;
    }

    public int MaxLife
    {
        get => maxLife;
    }

    public float FlickerInterval
    {
        get => flickerInterval;
    }

    
}
