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
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        
        
        if (Input.GetButtonDown(Utils.INPUT_UP))
        {
            OnMovementKeyPressed(Utils.INPUT_UP);
            OnKeyPressed();
        }
        else if (Input.GetButtonDown(Utils.INPUT_DOWN))
        {
            OnMovementKeyPressed(Utils.INPUT_DOWN);
            OnKeyPressed();

        }
        else if (Input.GetButtonDown(Utils.INPUT_LEFT))
        {
            OnMovementKeyPressed(Utils.INPUT_LEFT);
            OnKeyPressed();

        }
        else if (Input.GetButtonDown(Utils.INPUT_RIGHT))
        {
            OnMovementKeyPressed(Utils.INPUT_RIGHT);
            OnKeyPressed();

        }
        else if (Input.GetButtonDown(Utils.INPUT_EXTINGUISH))
        {
            OnInteractionKeyPressed();
            OnKeyPressed();

        }

    }
}
