using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [FormerlySerializedAs("settings")] [SerializeField] private GameplaySettings gameplaySettings; 
    [SerializeField] private int rounds;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private List<Vector3> possibleFirePositions;
    public int Rounds => rounds;
    public GameplaySettings GameplaySettings => gameplaySettings;

    private int _activeFires = 0;
    private PlayerController _player;
    
    public delegate void GameWonAction();
    public delegate void GameLostAction();

    public event GameWonAction OnGameWon;
    public event GameLostAction OnGameLost;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rounds = 0;
        _player = FindObjectOfType<PlayerController>();

        InitFirePositions();
    }

//    private void Update()
//    {
//        if(Input.GetKeyDown(KeyCode.F))
//            SpawnFire();
//    }

    private void OnEnable()
    {
        InputManager.Instance.OnKeyPressed += IncreaseRounds;
        Fire.OnFireKilled += AddFirePossiblePosition;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnKeyPressed -= IncreaseRounds;
        Fire.OnFireKilled += AddFirePossiblePosition;

    }

    private void IncreaseRounds()
    {
        if(++rounds % gameplaySettings.RoundsUntilNewFire == 0)
            SpawnFire();
    }

    private void SpawnFire()
    {
        var positions = possibleFirePositions.Count;

        if (positions == 0)
            return;
        
        bool allowPosition = false;
        var r = 0;
        var spawnPos = Vector3.zero;

        while (!allowPosition)
        {
            r = Random.Range(0, positions - 1);
            spawnPos = possibleFirePositions[r];
            allowPosition = spawnPos != _player.transform.position;
        }

        RemovePossibleFirePosition(spawnPos);
        Instantiate(firePrefab, spawnPos, Quaternion.identity, transform);
    }

    private void InitFirePositions()
    {
        wallTilemap.CompressBounds();

        var cellBounds = wallTilemap.cellBounds;

        possibleFirePositions = new List<Vector3>();

        for (var i = cellBounds.xMin+1; i < cellBounds.xMax - 1; i++)
        {
            for (var j = cellBounds.yMin+1; j < cellBounds.yMax - 1; j++)
            {
                AddFirePossiblePosition(new Vector3(i, j, 0));
            }
        }
    }

    private void RemovePossibleFirePosition(Vector3 pos)
    {
//        Debug.Log("removed: " + pos);

        possibleFirePositions.Remove(pos);
        ++_activeFires;
    }

    private void AddFirePossiblePosition(Vector3 pos)
    {
//        Debug.Log("added: " + pos);
        possibleFirePositions.Add(pos);
        if (--_activeFires == 0 && rounds > gameplaySettings.RoundsUntilNewFire)
        {
            OnGameWon();
        }
    }
}
