using System;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private int life = 3;
    [SerializeField] private float flickerInterval;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        InvokeRepeating(nameof(Flicker), 0, flickerInterval);
    }

    private void Flicker()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }
    
    public void Decrease()
    {
        transform.GetChild(0).localScale -= Vector3.one/3;
        if(--life <= 0)
            Kill();
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
