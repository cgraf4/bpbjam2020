using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int currentStage = 0;
    
    public delegate void GameWonAction();
    public delegate void GameLostAction();
    public delegate void StageUpAction(int stage);
    public delegate void StageDownAction(int stage);

    public event GameWonAction OnGameWon;
    public event GameLostAction OnGameLost;
    public event StageUpAction OnStageUp;
    public event StageDownAction OnStageDown;


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
//            Debug.Log("spawnPos (" + spawnPos + ") player pos (" + _player.transform.position + ") ->" + allowPosition);
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

        _activeFires = 0;
    }

    private void RemovePossibleFirePosition(Vector3 pos)
    {
//        Debug.Log("removed: " + pos);

        possibleFirePositions.Remove(pos);
        Debug.Log("active fires:" + _activeFires + " | current stage+1: " +(currentStage+1) + " | val: " + gameplaySettings.Stages[currentStage + 1]);
        if (++_activeFires >= gameplaySettings.Stages[currentStage])
        {
            OnStageUp(currentStage++);
//            currentStage++;
        }
    }
        

    private void AddFirePossiblePosition(Vector3 pos)
    {
//        Debug.Log("added: " + pos);
        possibleFirePositions.Add(pos);
        if (--_activeFires == 0 && rounds > gameplaySettings.RoundsUntilNewFire)
        {
            StartCoroutine(WinGame());
        }

        if (_activeFires <= gameplaySettings.Stages[currentStage] && rounds > 0)
        {
//            currentStage--;
            OnStageDown(currentStage--);
        }
    }

    private IEnumerator WinGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Win");
    }

    private IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Lose");
    }
    
}
