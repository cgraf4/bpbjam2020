using System;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public delegate void KillFireAction(Vector3 pos);
    public static event KillFireAction OnFireKilled;
    
    [SerializeField] private int maxLife = 3;
    [SerializeField] private float flickerInterval;

    private int _life = 1;
    private SpriteRenderer _spriteRenderer;
    private Transform _firstChild;
    private float _scale;

    private void Start()
    {
        _firstChild = transform.GetChild(0);
        _scale = 1/(float)maxLife;
        _firstChild.localScale = new Vector3(_scale, _scale, 1);

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        InvokeRepeating(nameof(Flicker), 0, flickerInterval);

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
        _scale = Math.Max(_scale - 1/(maxLife/2f), 0);
        _firstChild.localScale = new Vector3(_scale, _scale, 1);

        _life -= 2;
        if(_life <= 0)
            Kill();
    }
    
    public void Increase()
    {
        _scale = Math.Min(_scale + (1 / (float)maxLife), 1);
        _firstChild.localScale = new Vector3(_scale, _scale, 1);
        
        if (_life < maxLife)
            ++_life;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
