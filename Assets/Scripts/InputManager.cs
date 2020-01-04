using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void KeyPressAction(string key);
    public static event KeyPressAction OnKeyPressed;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown(Utils.MOVE_UP))
            OnKeyPressed(Utils.MOVE_UP);
        else if (Input.GetButtonDown(Utils.MOVE_DOWN))
            OnKeyPressed(Utils.MOVE_DOWN);
        else if (Input.GetButtonDown(Utils.MOVE_LEFT))
            OnKeyPressed(Utils.MOVE_LEFT);
        else if (Input.GetButtonDown(Utils.MOVE_RIGHT))
            OnKeyPressed(Utils.MOVE_RIGHT);

    }
}
