using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private int life = 3;

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
