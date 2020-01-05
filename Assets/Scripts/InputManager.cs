using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    public delegate void MovementKeyPressAction(string key);
    public delegate void InteractionKeyPressAction();
    public delegate void KeyPressAction();
    public event MovementKeyPressAction OnMovementKeyPressed;
    public event InteractionKeyPressAction OnInteractionKeyPressed;
    public event KeyPressAction OnKeyPressed;
    public bool canMove;
    public delegate void InactiveAction();
    public event InactiveAction OnInactive;

    private float timeLastPressedButton = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);

//        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        
        if (Time.timeSinceLevelLoad > timeLastPressedButton + 10)
        {
            timeLastPressedButton = Time.timeSinceLevelLoad;
//            Debug.Log("inactive");
            OnInactive();
        }
    }

    private void HandleInput()
    {
        if (!canMove)
            return;
        
        if (Input.GetButtonDown(Utils.INPUT_UP))
        {
            timeLastPressedButton = Time.timeSinceLevelLoad;

            OnMovementKeyPressed(Utils.INPUT_UP);
            OnKeyPressed();
        }
        else if (Input.GetButtonDown(Utils.INPUT_DOWN))
        {
            timeLastPressedButton = Time.timeSinceLevelLoad;
            OnMovementKeyPressed(Utils.INPUT_DOWN);
            OnKeyPressed();

        }
        else if (Input.GetButtonDown(Utils.INPUT_LEFT))
        {
            timeLastPressedButton = Time.timeSinceLevelLoad;
            OnMovementKeyPressed(Utils.INPUT_LEFT);
            OnKeyPressed();

        }
        else if (Input.GetButtonDown(Utils.INPUT_RIGHT))
        {
            timeLastPressedButton = Time.timeSinceLevelLoad;
            OnMovementKeyPressed(Utils.INPUT_RIGHT);
            OnKeyPressed();

        }
        else if (Input.GetButtonDown(Utils.INPUT_EXTINGUISH))
        {
            timeLastPressedButton = Time.timeSinceLevelLoad;
            OnInteractionKeyPressed();
            OnKeyPressed();

        }

    }
}
