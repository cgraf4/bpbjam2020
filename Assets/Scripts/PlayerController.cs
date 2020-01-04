using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask movementMask;
    [SerializeField] private LayerMask fireMask;

    private Vector3 _initialScale;
    private Transform _firstChild;

    private void Start()
    {
        _firstChild = transform.GetChild(0);
        _initialScale = _firstChild.localScale;
    }

    private void OnEnable()
    {
        InputManager.OnMovementKeyPressed += Move;
        InputManager.OnInteractionKeyPressed += Extinguish;
    }

    private void OnDisable()
    {
        InputManager.OnMovementKeyPressed -= Move;
        InputManager.OnInteractionKeyPressed -= Extinguish;
    }

    private void Move(string key)
    {
        Vector3 moveDir = Vector3.zero;
        Vector2 rayOrigin = transform.position+new Vector3(.5f, .5f, 0f);
        Vector2 rayDir = Vector2.zero;
        
        if (key == Utils.INPUT_DOWN)
        {
            rayDir = moveDir = Vector2.down;
        }
        else if (key == Utils.INPUT_UP)
        {
            rayDir = moveDir = Vector2.up;
        }
        else if (key == Utils.INPUT_LEFT)
        {
            rayDir = moveDir = Vector2.left;
            _firstChild.localScale = new Vector3(-_initialScale.x, _initialScale.y, _initialScale.z);
        }
        else if (key == Utils.INPUT_RIGHT)
        {
            rayDir = moveDir = Vector2.right;
            _firstChild.localScale = _initialScale;

        }

        Debug.DrawRay(rayOrigin, rayDir, Color.green);


        RaycastHit2D hitInfo = Physics2D.Raycast(rayOrigin, rayDir, 1, movementMask);
        
        if(hitInfo.collider == null)
            transform.position += moveDir;
        else
            Debug.Log(hitInfo.transform.name);
        
    }

    private void Extinguish()
    {
        Vector2 rayOrigin = transform.position+new Vector3(.5f, .5f, 0f);

        LookForFire(rayOrigin, Vector2.down, 1, fireMask);
        LookForFire(rayOrigin, Vector2.up, 1, fireMask);
        LookForFire(rayOrigin, Vector2.left, 1, fireMask);
        LookForFire(rayOrigin, Vector2.right, 1, fireMask);
    }

    private void LookForFire(Vector2 origin, Vector2 dir, float distance, LayerMask mask)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, dir, distance, mask);
        
        if (hitInfo.collider != null)
            hitInfo.transform.GetComponent<Fire>().Decrease();
        
        Debug.DrawRay(origin, dir, Color.blue);
    }
    
}
