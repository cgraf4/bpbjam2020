using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int rounds;
    [SerializeField] private int roundsUntilNewFire;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Tilemap wallTilemap;

    [SerializeField] private List<Vector3> possibleFirePositions;
    
    public int Rounds => rounds;

    public int RoundsUntilNewFire => roundsUntilNewFire;

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
        Fire.OnFireKilled += AddFirePosition;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnKeyPressed -= IncreaseRounds;
        Fire.OnFireKilled += AddFirePosition;

    }

    private void IncreaseRounds()
    {
        if(++rounds % RoundsUntilNewFire == 0)
            SpawnFire();
    }

    private void SpawnFire()
    {
        var positions = possibleFirePositions.Count;

        if (positions == 0)
            return;
        
        var r = Random.Range(0, positions - 1);
        var spawnPos = possibleFirePositions[r];
        RemoveFirePosition(spawnPos);
        var tempFire = Instantiate(firePrefab, spawnPos, Quaternion.identity, transform);
        
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
                possibleFirePositions.Add(new Vector3(i, j, 0));
            }
        }
    }

    private void RemoveFirePosition(Vector3 pos)
    {
        possibleFirePositions.Remove(pos);
    }

    private void AddFirePosition(Vector3 pos)
    {
        possibleFirePositions.Add(pos);
    }
}
