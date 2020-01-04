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
        cf.useLayerMask = true;
        cf.layerMask = fireLayerMask;
        cf.useTriggers = true;
        List<RaycastHit2D> hitInfos = new List<RaycastHit2D>();

        Physics2D.Raycast(transform.position, Vector2.right, cf, hitInfos, 50);

        Debug.Log("fires: " + hitInfos.Count);
        Debug.DrawRay(transform.position, Vector3.right, Color.red, 2f);
        foreach (var fire in hitInfos)
        {
            Debug.Log(fire.transform.name);
        }
        
        if (hitInfos.Count >= 8)
            StartCoroutine(GameManager.Instance.LoseGame());
    }
}
