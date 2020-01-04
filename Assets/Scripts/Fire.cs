using System;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public delegate void KillFireAction(Vector3 pos);
    public static event KillFireAction OnFireKilled;

    private int _life = 1;
    private SpriteRenderer _spriteRenderer;
    private Transform _firstChild;
    private float _scale;

    private void Start()
    {
        _firstChild = transform.GetChild(0);
        _scale = 1 / (float) GameManager.Instance.GameplaySettings.MaxLife;
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
        _scale = Math.Max(_scale - 1/(GameManager.Instance.GameplaySettings.MaxLife/2f), 0);
        _firstChild.localScale = new Vector3(_scale, _scale, 1);

        _life -= 2;
        if(_life <= 0)
            Kill();
    }
    
    public void Increase()
    {
        _scale = Math.Min(_scale + (1 / (float)GameManager.Instance.GameplaySettings.MaxLife), 1);
        _firstChild.localScale = new Vector3(_scale, _scale, 1);
        
        if (_life < GameManager.Instance.GameplaySettings.MaxLife)
            ++_life;
    }

    private void Kill()
    {
        OnFireKilled(transform.position);
        Destroy(gameObject);
    }
}
