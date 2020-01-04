using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void MovementKeyPressAction(string key);
    public delegate void InteractionKeyPressAction();
    public static event MovementKeyPressAction OnMovementKeyPressed;
    public static event InteractionKeyPressAction OnInteractionKeyPressed;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown(Utils.INPUT_UP))
            OnMovementKeyPressed(Utils.INPUT_UP);
        else if (Input.GetButtonDown(Utils.INPUT_DOWN))
            OnMovementKeyPressed(Utils.INPUT_DOWN);
        else if (Input.GetButtonDown(Utils.INPUT_LEFT))
            OnMovementKeyPressed(Utils.INPUT_LEFT);
        else if (Input.GetButtonDown(Utils.INPUT_RIGHT))
            OnMovementKeyPressed(Utils.INPUT_RIGHT);
        else if (Input.GetButtonDown(Utils.INPUT_EXTINGUISH))
            OnInteractionKeyPressed();

    }
}
