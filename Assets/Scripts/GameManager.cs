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

    public List<Vector3> PossibleFirePositions => possibleFirePositions;

    private int _activeFires = 0;
    private PlayerController _player;
    private int currentStage = 0;
    
    public delegate void StageUpAction(int stage);
    public delegate void StageDownAction(int stage);
    public delegate void SpawnFireAction();
    public event StageUpAction OnStageUp;
    public event StageDownAction OnStageDown;
    public event SpawnFireAction OnFireSpawned;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);
        
//        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rounds = 0;
        _player = FindObjectOfType<PlayerController>();

        InitFirePositions();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(WinGame());
    }

    private void OnEnable()
    {
        InputManager.Instance.OnKeyPressed += IncreaseRounds;
        Fire.OnFireKilled += AddFirePossiblePosition;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnKeyPressed -= IncreaseRounds;
        Fire.OnFireKilled -= AddFirePossiblePosition;

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
        OnFireSpawned();
    }
    
    public void SpawnFire(Vector3 pos)
    {
        RemovePossibleFirePosition(pos);
        Instantiate(firePrefab, pos, Quaternion.identity, transform);
        OnFireSpawned();
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

    public void RemovePossibleFirePosition(Vector3 pos)
    {
//        Debug.Log("removed: " + pos);

        possibleFirePositions.Remove(pos);

        currentStage = Mathf.Max(currentStage, 0);
        Debug.Log("currentstage: " + currentStage);
        Debug.Log("gameplaySettings.Stages[currentStage]: " + gameplaySettings.Stages[currentStage]);
//        Debug.Log("active fires:" + _activeFires + " | current stage: " +(currentStage) + " | val: " + gameplaySettings.Stages[currentStage]);
        if (++_activeFires >= gameplaySettings.Stages[currentStage])
        {
            if(currentStage<gameplaySettings.Stages.Length-1)
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

//        Debug.Log("currentstage: " + currentStage);
//        Debug.Log("gameplaySettings.Stages[currentStage]: " + gameplaySettings.Stages[currentStage]);
        if (_activeFires <= gameplaySettings.Stages[currentStage] && rounds > 0)
        {
//            currentStage--;
            OnStageDown(currentStage--);
        }
    }

    private IEnumerator WinGame()
    {
        Fire[] fires = FindObjectsOfType<Fire>();
        int f = fires.Length;
        for (int i = 0; i < f; i++)
        {
            Destroy(fires[i].gameObject);
        }
        
        FindObjectOfType<CutSceneManager>().PlayAnimation();
        FindObjectOfType<InputManager>().canMove = false;
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("Lose");
    }
    
}
