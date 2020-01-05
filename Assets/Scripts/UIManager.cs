using System;
using UnityEngine;
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
        
//        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
//        _maxDots = GameManager.Instance.GameplaySettings.RoundsUntilNewFire;
//        _dotImages = new GameObject[_maxDots];
//        
//        for (var i = 0; i < _maxDots; i++)
//        {
//            _dotImages[i] = Instantiate(dotsImagePrefab, dotsLayoutGroup);
//            _dotImages[i].SetActive(false);
//        }
    }

//    private void OnEnable()
//    {
//        InputManager.Instance.OnKeyPressed += UpdateRoundCounter;
//    }
//
//    private void OnDisable()
//    {
//        InputManager.Instance.OnKeyPressed -= UpdateRoundCounter;
//    }

    private void UpdateRoundCounter()
    {
        var dotsToDisplay = GameManager.Instance.Rounds % _maxDots;
        
        for (var i = 0; i < _maxDots; i++)
        {
            _dotImages[i].SetActive(i < dotsToDisplay);
        }
    }

    private bool gamePaused = false;
    public GameObject img;
    private void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
        }

        if (gamePaused)
        {
            img.SetActive(true);
            Time.timeScale = 0;
            if(Input.GetKeyUp(KeyCode.Y))
                Application.Quit();
            else if (Input.GetKeyDown((KeyCode.N)))
                gamePaused = false;
        }
        else
        {
            Time.timeScale = 1;
            img.SetActive(false);

        }
#endif
    }
}
