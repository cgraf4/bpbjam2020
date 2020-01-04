using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCheck : MonoBehaviour
{
    public LayerMask fireLayerMask;
    private void OnEnable()
    {
        GameManager.Instance.OnFireSpawned += CheckForFireLine;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnFireSpawned -= CheckForFireLine;

    }

    private void CheckForFireLine()
    {
        ContactFilter2D cf = new ContactFilter2D();
        cf.NoFilter();
        List<RaycastHit2D> hitInfos = new List<RaycastHit2D>();

        Physics2D.Raycast(transform.position, Vector2.right, cf, hitInfos, 50);

        if (hitInfos.Count == 8)
            StartCoroutine(GameManager.Instance.LoseGame());
    }
}
