﻿using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    
    [SerializeField] private Transform dotsLayoutGroup;
    [FormerlySerializedAs("DotsImagePrefab")] [SerializeField] private GameObject dotsImagePrefab;

    private GameObject[] _dotImages;

    private int _maxDots;
    
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        _maxDots = GameManager.Instance.RoundsUntilNewFire;
        _dotImages = new GameObject[_maxDots];
        
        for (var i = 0; i < _maxDots; i++)
        {
            _dotImages[i] = Instantiate(dotsImagePrefab, dotsLayoutGroup);
            _dotImages[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        InputManager.OnMovementKeyPressed += UpdateRoundCounter;
    }

    private void OnDisable()
    {
        InputManager.OnMovementKeyPressed -= UpdateRoundCounter;
    }

    private void UpdateRoundCounter(string s)
    {
        var dotsToDisplay = GameManager.Instance.Rounds % _maxDots;
        
        for (var i = 0; i < _maxDots; i++)
        {
            _dotImages[i].SetActive(i < dotsToDisplay);
        }
    }

}