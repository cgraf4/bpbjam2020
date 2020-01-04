using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask movementMask;
    [SerializeField] private LayerMask fireMask;

    private Vector3 _initialScale;
    private Transform _firstChild;

    private Vector3 _moveDir = Vector3.zero;

    Vector2 _moveRayOrigin = Vector2.zero;
    Vector2 _moveRayDir = Vector2.zero;
    Vector2 _fireRayOrigin = Vector2.zero; 
    Vector2 _fireRayDir = Vector2.zero;
    
    private void Start()
    {
        _firstChild = transform.GetChild(0);
        _initialScale = _firstChild.localScale;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnMovementKeyPressed += Move;
        InputManager.Instance.OnInteractionKeyPressed += Extinguish;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMovementKeyPressed -= Move;
        InputManager.Instance.OnInteractionKeyPressed -= Extinguish;
    }

    private void Move(string key)
    {
        _moveRayOrigin = transform.position+new Vector3(.5f, .5f, 0f);
        _moveRayDir = Vector2.zero;
        
        if (key == Utils.INPUT_DOWN)
        {
            _moveRayDir = _moveDir = Vector2.down;
        }
        else if (key == Utils.INPUT_UP)
        {
            _moveRayDir = _moveDir = Vector2.up;
        }
        else if (key == Utils.INPUT_LEFT)
        {
            _moveRayDir = _moveDir = _fireRayDir = Vector2.left;
            _firstChild.localScale = new Vector3(-_initialScale.x, _initialScale.y, _initialScale.z);
        }
        else if (key == Utils.INPUT_RIGHT)
        {
            _moveRayDir = _moveDir = _fireRayDir =  Vector2.right;
            _firstChild.localScale = _initialScale;

        }

//        Debug.DrawRay(_moveRayOrigin, _moveRayDir, Color.green);


        RaycastHit2D hitInfo = Physics2D.Raycast(_moveRayOrigin, _moveRayDir, 1, movementMask);
        
        if(hitInfo.collider == null)
            transform.position += _moveDir;
//        else
//            Debug.Log(hitInfo.transform.name);
        
    }

    private void Extinguish()
    {
        Vector2 fireRayOrigin = transform.position+new Vector3(.5f, .5f, 0f);

//        LookForFire(rayOrigin, Vector2.down, 1, fireMask);
//        LookForFire(rayOrigin, Vector2.up, 1, fireMask);
//        LookForFire(rayOrigin, Vector2.left, 1, fireMask);
//        LookForFire(rayOrigin, Vector2.right, 1, fireMask);
        
//        if(_moveDir == Vector3.left || _moveDir == Vector3.right)
            LookForFire(fireRayOrigin, _fireRayDir, 1, fireMask);
    }

    private void LookForFire(Vector2 origin, Vector2 dir, float distance, LayerMask mask)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, dir, distance, mask);
        
        if (hitInfo.collider != null)
            hitInfo.transform.GetComponent<Fire>().Decrease();
        
//        Debug.DrawRay(origin, dir, Color.blue);
    }
    
}
