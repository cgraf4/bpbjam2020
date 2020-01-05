using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Fire : MonoBehaviour
{
    public delegate void KillFireAction(Vector3 pos);
    public static event KillFireAction OnFireKilled;

    private int _life = 1;
    private int _uberLife = 1;
    private SpriteRenderer _spriteRenderer;
    private Transform _firstChild;
    private float _scale;

    public LayerMask fireLayerMask;

    private void Start()
    {
        _firstChild = transform.GetChild(0);
        _scale = 1 / (float) GameManager.Instance.GameplaySettings.FireMaxLife;
        _firstChild.localScale = new Vector3(_scale, _scale, 1);

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        InvokeRepeating(nameof(Flicker), 0, GameManager.Instance.GameplaySettings.FlickerInterval);
        
    }
    
    private void OnEnable()
    {
        InputManager.Instance.OnKeyPressed += Increase;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnKeyPressed -= Increase;
    }

    private void Flicker()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }
    
    public void Decrease()
    {
        _scale = Math.Max(_scale - 1/(GameManager.Instance.GameplaySettings.FireMaxLife/2f), 0);
        _firstChild.localScale = new Vector3(_scale, _scale, 1);

        _life -= 2;
        if(_life <= 0)
            Kill();
    }
    
    public void Increase()
    {
        _scale = Math.Min(_scale + (1 / (float)GameManager.Instance.GameplaySettings.FireMaxLife), 1);
        _firstChild.localScale = new Vector3(_scale, _scale, 1);

        if (_life < GameManager.Instance.GameplaySettings.FireMaxLife)
        {
            ++_life;
            _uberLife = 0;
        }
        else if (_life == GameManager.Instance.GameplaySettings.FireMaxLife && _uberLife < GameManager.Instance.GameplaySettings.FireUberLife)
        {
            Debug.Log("max life");
            _uberLife++;
        }
        else if(_uberLife == GameManager.Instance.GameplaySettings.FireUberLife)
        {
            Debug.Log("uber");
            List<Vector3> emptySpaces = new List<Vector3>();

            Collider2D temp;

            if (LookForFire(transform.position, Vector2.down, 1, fireLayerMask) == null)
            {
                emptySpaces.Add(transform.position+Vector3.down);
            }
            if (LookForFire(transform.position, Vector2.up, 1, fireLayerMask) == null)
            {
                emptySpaces.Add(transform.position+Vector3.up);
            }
            if (LookForFire(transform.position, Vector2.left, 1, fireLayerMask) == null)
            {
                emptySpaces.Add(transform.position+Vector3.left);
            }
            if (LookForFire(transform.position, Vector2.right, 1, fireLayerMask) == null)
            {
                emptySpaces.Add(transform.position+Vector3.right);
            }

            bool mustFindPosition = true;

            while (mustFindPosition)
            {
                if (emptySpaces.Count == 0)
                    return;

                int r = UnityEngine.Random.Range(0, emptySpaces.Count - 1);
                
                if (GameManager.Instance.PossibleFirePositions.Contains(emptySpaces[r]))
                {
                    Debug.Log("spawn fire at: " + emptySpaces[r]);
                    GameManager.Instance.SpawnFire(emptySpaces[r]);
                    mustFindPosition = false;
                }
                else
                {
                    emptySpaces.RemoveAt(r);
                }
                    
            }
            

        }
    }

    private void Kill()
    {
        OnFireKilled(transform.position);
        Destroy(gameObject);
    }
    
    private Collider2D LookForFire(Vector2 origin, Vector2 dir, float distance, LayerMask mask)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, dir, distance, mask);

        return hitInfo.collider;

//        Debug.DrawRay(origin, dir, Color.blue);
    }
    
    
}
