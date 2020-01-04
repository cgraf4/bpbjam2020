using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int rounds;
    [SerializeField] private int roundsUntilNewFire;
    [SerializeField] private GameObject firePrefab;
    
    public int Rounds => rounds;

    public int RoundsUntilNewFire => roundsUntilNewFire;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rounds = 0;
    }
    
    private void OnEnable()
    {
        InputManager.OnMovementKeyPressed += IncreaseRounds;
    }

    private void OnDisable()
    {
        InputManager.OnMovementKeyPressed -= IncreaseRounds;
    }

    private void IncreaseRounds(string s)
    {
        ++rounds;
    }

    private void SpawnFire()
    {
        GameObject tempFire = Instantiate(firePrefab, transform);
        
//        Tilemap map = 
    }
}
