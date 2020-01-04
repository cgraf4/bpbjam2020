using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask movementMask;
    private void OnEnable()
    {
        InputManager.OnKeyPressed += Move;
    }

    private void OnDisable()
    {
        InputManager.OnKeyPressed -= Move;
    }

    private void Move(string key)
    {
        Vector3 moveDir = Vector3.zero;
        Vector2 rayOrigin = transform.position+new Vector3(.5f, .5f, 0f);
        Vector2 rayDir = Vector2.zero;
        
        if (key == Utils.MOVE_DOWN)
        {
            rayDir = moveDir = Vector2.down;
        }
        else if (key == Utils.MOVE_UP)
        {
            rayDir = moveDir = Vector2.up;
        }
        else if (key == Utils.MOVE_LEFT)
        {
            rayDir = moveDir = Vector2.left;
        }
        else if (key == Utils.MOVE_RIGHT)
        {
            rayDir = moveDir = Vector2.right;
        }

        Debug.DrawRay(rayOrigin, rayDir, Color.green);


        RaycastHit2D hitInfo = Physics2D.Raycast(rayOrigin, rayDir, 1, movementMask);
        
        if(hitInfo.collider == null)
            transform.position += moveDir;
        else
            Debug.Log(hitInfo.transform.name);
        
    }
}
