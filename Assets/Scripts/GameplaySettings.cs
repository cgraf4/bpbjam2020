using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Gameplay Settings", menuName = "Settings/Gameplay Settings", order = 1)]

public class GameplaySettings : ScriptableObject
{
    [SerializeField] private int roundsUntilNewFire = 5;
    [FormerlySerializedAs("maxLife")] [SerializeField] private int fireFireMaxLife = 3;
    [SerializeField] private int fireUberLife = 3;
    [SerializeField] private float flickerInterval = .1f;
    [SerializeField] private int[] stages;

    public int[] Stages => stages;
    public int RoundsUntilNewFire => roundsUntilNewFire;

    public int FireMaxLife => fireFireMaxLife;

    public int FireUberLife => fireUberLife;

    public float FlickerInterval => flickerInterval;
}
